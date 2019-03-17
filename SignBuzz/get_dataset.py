import cv2
import os

alphabet = ['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'del','nothing','space']

cap = cv2.VideoCapture(0)
start_saving = False

current_letter = 0
n_taken= 0
write_img_dict = {}

while True:
    ret, frame = cap.read()
    h, w = frame.shape[:2]
    cv2.rectangle(frame, (100-3,100-3), (300+3,300+3), (255,0,0), 3)
    cv2.putText(frame, "{}".format(alphabet[current_letter]), (350, 100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 250, 0))
    if start_saving:
        cv2.putText(frame, "SAVING - {}".format(alphabet[current_letter]), (int(w/2-60),int(h-10)), cv2.FONT_HERSHEY_SIMPLEX, 3, (0,255,0))
        cv2.putText(frame, "Taken for letter - {}".format(n_taken), (int(w/2-60), 100),
                    cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255))
        gesture = frame[100:300,100:300]
        letter_dir = os.path.join("New_DS", alphabet[current_letter])
        img_path = os.path.join(letter_dir,"{}{}.jpg".format(alphabet[current_letter],n_taken))


        if not os.path.exists(letter_dir):
            os.makedirs(letter_dir)
        cv2.imwrite(os.path.join(letter_dir,"{}{}.jpg".format(alphabet[current_letter],n_taken)), gesture)
        n_taken+=1


    cv2.imshow("frame", frame)
    key = cv2.waitKey(1)
    if key == ord('s'):
        start_saving = True
    if key == ord('d'):
        start_saving = False
    if key == ord('n'):
        current_letter += 1
        n_taken = 0
    if key == ord('q'):
        break