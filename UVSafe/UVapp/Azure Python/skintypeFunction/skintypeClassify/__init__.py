import logging

import azure.functions as func

import cv2
import numpy as np
import math
import json
import base64

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    req_body = req.get_body()
    if not req_body:
        return func.HttpResponse(
            "Image missing",
            status_code=400
        )
    #jsonBody = req.get_json()
    #logging.info(f"Json {jsonBody[0:50]}")

    logging.info(f"len(req_body) = {len(req_body)} First bytes {req_body[0:50]}")

    decoded = base64.b64decode(req_body)
    logging.info(f"Decoded {decoded[0:50]}")

    #try:
    return classifySkintypeBytesImg(decoded)
    #except Exception as ex:
        #logging.info("failed raw bytes")

    '''
    decoded = base64.b64decode(req_body)
    
    try:
        return classifySkintypeBytesImg(decoded)
    except Exception as ex:
        logging.info("failed raw bytes")
        

    return func.HttpResponse(
        "Failed to decode",
        status_code=400
    )
    '''


def classifySkintypeBytesImg(data) -> func.HttpResponse:


    npArrayImg = np.frombuffer(data, dtype="uint8")

    logging.info(f"len(npArrayImg)={len(npArrayImg)}, first bytes: {npArrayImg[0:50]}")


    image = cv2.imdecode(npArrayImg, cv2.IMREAD_COLOR)

    skintype, color = classifySkinTypeFromImg(image)

    classifiedColor = {"skinType": skintype}

    responseBody = json.dumps(classifiedColor)

    return func.HttpResponse(body=responseBody)


def switchBR(img):
  r,g,b = cv2.split(img)
  return cv2.merge((b,g,r))

def switchBRvals(vals):
  return (vals[2], vals[1], vals[0])

def createSolidColorImg(colorComponents, width=16, height=16):
  img = np.zeros((width, height, 3), np.uint8)
  img[:,:] = (colorComponents[0], colorComponents[1], colorComponents[2])
  return img


lower4 = [0, 15, 0]; upper4 = [17, 170, 255] # Source: https://github.com/CHEREF-Mehdi/SkinDetection
lower_HSV_values = np.array(lower4, dtype = "uint8")
upper_HSV_values = np.array(upper4, dtype = "uint8")

lower_YCrCb_values = np.array([0, 135, 85], dtype="uint8")
upper_YCrCb_values = np.array([255,180,135], dtype="uint8")

lower_HSV_background_values1 = np.array([0, 0, 150], dtype = "uint8")
upper_HSV_background_values1 = np.array([255, 20, 255], dtype = "uint8")

lower_HSV_background_values2 = np.array([0, 0, 200])
upper_HSV_background_values2 = np.array([255, 50, 255])

# ITA definition and classification according to https://www.ncbi.nlm.nih.gov/pubmed/24098899
def classifySkintypeITA(ita):
    if (ita < -30):
        return 6
    elif -30 <= ita < 10:
        return 5
    elif 10 <= ita < 28:
        return 4
    elif 28 <= ita < 41:
        return 3
    elif 41 <= ita < 55:
        return 2
    elif 55 <= ita:
        return 1


def RGBtoLABvals(RGBvals):
    img = createSolidColorImg(RGBvals)
    labVals = cv2.cvtColor(switchBR(img), cv2.COLOR_BGR2LAB)[0, 0]
    return labVals


def convertColorspaceVals(vals, cvConstant):
    img = createSolidColorImg(vals)
    convertedVals = cv2.cvtColor(img, cvConstant)[0, 0]
    return convertedVals


def normalizeLabVals(labVals, roundVals=False):
    L, a, b = labVals
    L = L * 100.0 / 255
    a = (a - 128) * 100.0 / 128
    b = (b - 128) * 100.0 / 128

    if (roundVals):
        L, a, b = round(L), round(a), round(b)

    return (L, a, b)


def calcITA(vals, RGBnotLAB=False, normalizedLAB=True):
    labVals = None
    if (RGBnotLAB):
        labVals = normalizeLabVals(RGBtoLABvals(vals))
    else:
        if normalizedLAB:
            labVals = vals
        else:
            labVals = normalizeLabVals(vals)
    return math.degrees(math.atan2(labVals[0] - 50, labVals[2]))


def classifySkintypeLAB(labVals):
    ita = calcITA(labVals)
    return classifySkintypeITA(ita)


def classifySkintypeRGB(RGBvals):
    ita = calcITA(RGBvals, RGBnotLAB=True)
    return classifySkintypeITA(ita)


def adjustColorToBackgroundLighting(colorBGR, meanBackgroundColorHSV):
    meanColorHSV = convertColorspaceVals(colorBGR, cv2.COLOR_BGR2HSV)

    # adjustedValue =  meanColorHSV[2] + 255 - meanBackgroundColorHSV[2]
    # adjustedValue = round(meanColorHSV[2]*(1 + 2*(255-meanBackgroundColorHSV[2])/255.0))
    adjustedValue = 255 * (meanColorHSV[2] / meanBackgroundColorHSV[2])
    adjustedValue = np.clip(adjustedValue, 0, 255)
    adjustedMeanColorHSV = (meanColorHSV[0], meanColorHSV[1], adjustedValue)
    adjustedMeanColor = convertColorspaceVals(adjustedMeanColorHSV, cv2.COLOR_HSV2BGR)
    return adjustedMeanColor


def classifySkinTypeFromImg(img):
    # K-means quantization
    Z = img.reshape((-1, 3))
    Z = np.float32(Z)

    criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 10, 1.0)
    K = 5
    ret, label, center = cv2.kmeans(Z, K, None, criteria, 10, cv2.KMEANS_RANDOM_CENTERS)

    center = np.uint8(center)
    res = center[label.flatten()]
    res2 = res.reshape((img.shape))

    # Colorspaces
    img_hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    img_YCrCb = cv2.cvtColor(img, cv2.COLOR_BGR2YCrCb)

    img = res2

    # Colorspaces
    img_hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    img_YCrCb = cv2.cvtColor(img, cv2.COLOR_BGR2YCrCb)

    # Masking
    mask_hsv = cv2.inRange(img_hsv, lower_HSV_values, upper_HSV_values)
    mask_hsv = cv2.morphologyEx(mask_hsv, cv2.MORPH_OPEN, np.ones((3, 3), np.uint8))

    YCrCb_mask = cv2.inRange(img_YCrCb, lower_YCrCb_values, upper_YCrCb_values)
    YCrCb_mask = cv2.morphologyEx(YCrCb_mask, cv2.MORPH_OPEN, np.ones((3, 3), np.uint8))

    skinMask = cv2.bitwise_and(YCrCb_mask, mask_hsv)
    skinMask = cv2.morphologyEx(skinMask, cv2.MORPH_OPEN, np.ones((3, 3), np.uint8))
    skinMask = cv2.medianBlur(skinMask, 3)

    backgroundMask1 = cv2.inRange(img_hsv, lower_HSV_background_values1, upper_HSV_background_values1)
    backgroundMask1 = cv2.morphologyEx(backgroundMask1, cv2.MORPH_OPEN, np.ones((3, 3), np.uint8))
    backgroundMask2 = cv2.inRange(img_hsv, lower_HSV_background_values2, upper_HSV_background_values2)
    backgroundMask2 = cv2.morphologyEx(backgroundMask2, cv2.MORPH_OPEN, np.ones((3, 3), np.uint8))

    backgroundMask = cv2.bitwise_or(backgroundMask1, backgroundMask2)
    backgroundMask = cv2.medianBlur(backgroundMask, 3)

    skinMask = cv2.bitwise_and(skinMask, cv2.bitwise_not(backgroundMask))

    # Color classification calculations
    meanColor = cv2.mean(img, skinMask)
    meanBackgroundColor = cv2.mean(img, backgroundMask)

    # Probably should have used switchBRvals here but I don't want to change this now
    meanColorRGB = (round(meanColor[2], 1), round(meanColor[1], 1), round(meanColor[0], 1))
    meanBackgroundColorRGB = (
    round(meanBackgroundColor[2], 1), round(meanBackgroundColor[1], 1), round(meanBackgroundColor[0], 1))

    meanColorHSV = convertColorspaceVals(meanColor, cv2.COLOR_BGR2HSV)
    meanBackgroundColorHSV = convertColorspaceVals(meanBackgroundColor, cv2.COLOR_BGR2HSV)

    adjustedMeanColor = adjustColorToBackgroundLighting(meanColor, meanBackgroundColorHSV)
    skintype = classifySkintypeRGB(switchBRvals(adjustedMeanColor))

    return skintype, adjustedMeanColor