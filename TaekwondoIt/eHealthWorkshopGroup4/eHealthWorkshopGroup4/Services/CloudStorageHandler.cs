using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using eHealthWorkshopGroup4.Models;

namespace CloudStorage
{
    // TODO change availability after classses are in the right place
    public class DateEntity : TableEntity
    {
        /**
         * Windows Azure constant, see
         * https://stackoverflow.com/questions/14859405/azure-table-storage-returns-400-bad-request
         * and
         * https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model?redirectedfrom=MSDN
         */
        internal static DateTime DEFAULT_TIME = new DateTime(1601, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    }

    internal class ExerciseStorage
    {

        private const string ACCOUNT_NAME = "exercises";
        private const string ACCOUNT_KEY = "wWt4rzDXoF6FD/nFZpA6JxIgOYh6hWS41wc/CmwCs6LYc4dT/wI9Ct3AsJIf+U1uLB+/OoYjrBVZcXvzEYEfcA==";

        internal CloudTable exerciseTable;

        public ExerciseStorage()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
            new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                ACCOUNT_NAME, ACCOUNT_KEY), true);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Get a reference to a table named "uprof"
            exerciseTable = tableClient.GetTableReference("exercises");
        }

        internal class ExerciseEntity : TableEntity
        {
            internal Exercise generateExercise()
            {
                return new Exercise(GetStartTime(), PartitionKey, avgHR, peakHR, duration, exerciseName);
            }

            public ExerciseEntity() { }

            /**
             * @precond groupName is the name of the group of the user exer.uname
             */
            public ExerciseEntity(Exercise exer)
            {
                PartitionKey = exer.uname;
                RowKey = exer.startTime.Ticks.ToString();
                avgHR = exer.avgHR;
                peakHR = exer.peakHR;
                duration = exer.duration;
                exerciseName = exer.exerciseName;
            }

            internal DateTime GetStartTime() => new DateTime(long.Parse(RowKey));
            
            public double avgHR { get; set; }
            public double peakHR { get; set; }

            // in seconds ?
            public int duration { get; set; }
            public string exerciseName { get; set; }
        }

    }

    internal class PoomsaeStorage
    {
        private const string ACCOUNT_NAME = "poomsaes";
        private const string ACCOUNT_KEY = "o65gbI7Ykss3GLsEqNKkQu28K8U7aDJXMbKlE7t5mk5xnuCFQpOr4LLB8XxSDrl53RPbDI8RYt3bEWIwCz86RA==";

        internal CloudBlobContainer poomsaesContainer;

        public PoomsaeStorage()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
            new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                ACCOUNT_NAME, ACCOUNT_KEY), true);
            poomsaesContainer = storageAccount.CreateCloudBlobClient().GetContainerReference("taegeuks");
        }
    }

    public class UserStorage
    {

        private const string ACCOUNT_NAME = "usersinfo";
        private const string ACCOUNT_KEY = "fR/PSsSffvYp3Z7x6HXsk3sP5DVhI6qrmcJLoCCbucN+XrWdJOpheRvLNacO0qWSoI3DqXzg3axEDEQtoJXEAg==";

        internal CloudTable profileTable;
        internal CloudTable groupsTable;
        internal CloudTable msgsTable;
        internal CloudBlobClient cloudBlobClient;

        internal UserStorage()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
            new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                ACCOUNT_NAME, ACCOUNT_KEY), true);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Get a reference to a table named "uprof"
            profileTable = tableClient.GetTableReference("uprof");
            groupsTable = tableClient.GetTableReference("groups");
            msgsTable = tableClient.GetTableReference("messages");
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }


        public class MessageEntity : DateEntity
        {
            internal Message generateMessage()
            {
                return new Message(RowKey, PartitionKey, content, date);
            }

            public MessageEntity() { date = DEFAULT_TIME; }

            public MessageEntity(Message msg)
            {
                PartitionKey = msg.GroupName;
                RowKey = msg.Title;
                content = msg.Content;
                if (msg.Date < DEFAULT_TIME)
                    date = DEFAULT_TIME;
                else
                    date = msg.Date;
            }

            public string content { get; set; }
            public DateTime date { get; set; }

        }



        public class UserProfileEntity : DateEntity
        {
            /** Source: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/ */
            internal static string ComputeSha256Hash(string rawData)
            {
                if (rawData == null) return null;
                // Create a SHA256   
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            internal UserProfileData genrateUserProfileData()
            {
                return new UserProfileData(fullname, RowKey, (Rank)rank, isCoach, group, xp, lastTrainingDate);
            }

            internal static string getPartition(string uname) => uname[0].ToString().ToLower();

            public UserProfileEntity(UserProfileData upd, string password = null)
            {
                PartitionKey = getPartition(upd.uname);
                RowKey = upd.uname;
                fullname = upd.fullname;
                passhash = ComputeSha256Hash(password);
                rank = (int)upd.rank;
                isCoach = upd.isCoach;
                group = upd.group;
                xp = upd.xp;
                if (upd.lastTrainingDate < DEFAULT_TIME)
                    lastTrainingDate = DEFAULT_TIME;
                else
                    lastTrainingDate = upd.lastTrainingDate;
            }

            internal UserProfileEntity(string passhash, UserProfileData upd) : this(upd)
            {
                this.passhash = passhash;
            }

            public UserProfileEntity() { lastTrainingDate = DEFAULT_TIME; }
            public string fullname { get; set; }
            public string passhash { get; set; }
            public int rank { get; set; }

            // isCoach = true iff the user is a coach 
            public bool isCoach { get; set; }
            public string group { get; set; }
            public int xp { get; set; }

            public DateTime lastTrainingDate { get; set; }
        }

        public class GroupEntity : TableEntity
        {

            internal static string getGroupPartition(string group) => group[0].ToString().ToLower();

            public GroupEntity(string groupName, string coachName)
            {
                PartitionKey = getGroupPartition(groupName);
                RowKey = groupName;
                coach = coachName;
            }

            public GroupEntity() { }

            public string coach { get; set; }
        }
    }

    public class CloudStorageHandler
    {

        private static string getUserImageContainerName(string group, bool profile)
        {
            if (group == "") return profile ? "profile-images" : "background-images";
            return (profile ? "profile-images-" : "background-images-") + group.ToLower().Replace(' ', '-');
        }

        private CloudBlobContainer getUserImageContainer(string group, bool profile)
        {
            return userS.cloudBlobClient.GetContainerReference(getUserImageContainerName(group, profile));
        }



        private ExerciseStorage exerS;
        private PoomsaeStorage poomS;
        private UserStorage userS;

        public CloudStorageHandler()
        {
            exerS = new ExerciseStorage();
            poomS = new PoomsaeStorage();
            userS = new UserStorage();
        }


        /**
         * @precond password != null
         * @precond upd.uname[0] is an ASCII character
         * @predcond coachName is an existing user's username!
         * @precond upd.isCoach == true -> groupName != null
         *          because all coaches should train a group!
         * @param groupName is optional and will be treated iff upd.isCoach == true
         *        this parameter is the name of the group this user (coach) is training.
         *        
         * IMPORTANT this method should only be used after a succesful isUsernameTaken() call
         */
        public async Task addUserAsync(UserProfileData upd, string password, Stream profileImage, Stream backgroundImage, string groupName = null)
        {
            await userS.profileTable.ExecuteAsync(TableOperation.InsertOrReplace(new UserStorage.UserProfileEntity(upd, password)));

            // User profile and background pictures are stored in a block-blobs.
            // Each group has two blob containers (for profile and background pictures),
            // and inside each container, a block blob for each user in the group.
            // therefore, adding a coach user implies that two new blob containers will be created.
            if (upd.isCoach) await createTrainingGroup(groupName, upd.uname);

            CloudBlobContainer CBContainerProfile = getUserImageContainer(upd.group, true);
            CloudBlobContainer CBContainerBG = getUserImageContainer(upd.group, false);

            // TODO full path! under resources folder?
            if (profileImage == null) profileImage = pathToStream("BruceLeeProfile.png");
            await CBContainerProfile.GetBlockBlobReference(upd.uname).UploadFromStreamAsync(profileImage);
            // TODO full path of some default under resources folder
            if (backgroundImage == null) backgroundImage = pathToStream("background.png");
            await CBContainerBG.GetBlockBlobReference(upd.uname).UploadFromStreamAsync(backgroundImage);
        }

        public async Task<bool> isUsernameTaken(string uname)
        {
            return await getUserProfileData(uname) != null;
        }

        public async Task<bool> IsGroupExists(string group)
        {
            return null != await getGroupEntity(group);
        }



        /** 
         * @precond coachName is an existing user's username
         * @precond groupName isn't a name of an existing group
         */
        public async Task createTrainingGroup(string groupName, string coachName)
        {
            await userS.groupsTable.ExecuteAsync(TableOperation.InsertOrReplace(new UserStorage.GroupEntity(groupName, coachName)));

            CloudBlobContainer CBContainerGroupProfile = getUserImageContainer(groupName, true);
            CloudBlobContainer CBContainerGroupBG = getUserImageContainer(groupName, false);
            await CBContainerGroupProfile.CreateIfNotExistsAsync();
            await CBContainerGroupBG.CreateIfNotExistsAsync();

            // TODO set permissions public?
            await CBContainerGroupProfile.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            await CBContainerGroupBG.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }


        private async Task<UserStorage.GroupEntity> getGroupEntity(string group)
        {
            return (UserStorage.GroupEntity)(await userS.groupsTable.ExecuteAsync(TableOperation.Retrieve<UserStorage.GroupEntity>(UserStorage.GroupEntity.getGroupPartition(group), group))).Result;
        }

        private async Task<UserStorage.UserProfileEntity> getUserProfileEntity(string uname)
        {
            return (UserStorage.UserProfileEntity)(await userS.profileTable.ExecuteAsync(TableOperation.Retrieve<UserStorage.UserProfileEntity>(UserStorage.UserProfileEntity.getPartition(uname), uname))).Result;
        }

        public async Task<UserProfileData> getUserProfileData(string uname)
        {
            return (await getUserProfileEntity(uname))?.genrateUserProfileData();
        }

        /** Get a user's coach (return's the coach's username) */
        public async Task<string> getUserCoach(string uname)
        {
            return await getGroupCoach((await getUserProfileEntity(uname)).group);
        }

        public async Task updateUserRankAsync(string uname, Rank r)
        {
            UserStorage.UserProfileEntity enti = await getUserProfileEntity(uname);
            UserProfileData upd = enti?.genrateUserProfileData();
            upd.rank = r;  // change the rank
            UserStorage.UserProfileEntity updateEnti = new UserStorage.UserProfileEntity(enti.passhash, upd);
            updateEnti.ETag = "*";
            await userS.profileTable.ExecuteAsync(TableOperation.Replace(updateEnti));
        }

        public async Task updateUserLastTrainingDateAsync(string uname, DateTime LTD)
        {
            UserStorage.UserProfileEntity enti = await getUserProfileEntity(uname);
            UserProfileData upd = enti?.genrateUserProfileData();
            if (upd.lastTrainingDate < LTD) upd.lastTrainingDate = LTD;  // change LTD
            UserStorage.UserProfileEntity updateEnti = new UserStorage.UserProfileEntity(enti.passhash, upd);
            updateEnti.ETag = "*";
            await userS.profileTable.ExecuteAsync(TableOperation.Replace(updateEnti));
        }

        /** NOTE: bonus isn't the new xp. The given value is *added* to the old xp */
        public async Task incrUserXPAsync(string uname, int bonus)
        {
            UserStorage.UserProfileEntity enti = await getUserProfileEntity(uname);
            UserProfileData upd = enti?.genrateUserProfileData();
            upd.xp += bonus;  // update the xp
            UserStorage.UserProfileEntity updateEnti = new UserStorage.UserProfileEntity(enti.passhash, upd);
            updateEnti.ETag = "*";
            await userS.profileTable.ExecuteAsync(TableOperation.Replace(updateEnti));
        }


        /**
         * IMPORTANT !!!
         * this method should be used only after user authenticated, i.e after a succesful return of
         * userPasswordQueryAsync() method!
         */
        public async Task updateUserPasswordAsync(string uname, string pass)
        {
            UserProfileData upd = await getUserProfileData(uname);
            UserStorage.UserProfileEntity enti = new UserStorage.UserProfileEntity(upd, pass);
            enti.ETag = "*";
            await userS.profileTable.ExecuteAsync(TableOperation.Replace(enti));
        }

        /** Don't remove a coach user! */
        public async Task removeUserAsync(string uname)
        {
            int delCount = 0;
            bool exerciseExists;
            TableBatchOperation batchOp = new TableBatchOperation();
            UserStorage.UserProfileEntity enti = await getUserProfileEntity(uname);
            UserProfileData upd = enti.genrateUserProfileData();
            string group = upd.group;

            await userS.profileTable.ExecuteAsync(TableOperation.Delete(enti));
            await getUserImageContainer(group, true).GetBlockBlobReference(uname).DeleteIfExistsAsync();
            await getUserImageContainer(group, false).GetBlockBlobReference(uname).DeleteIfExistsAsync();

            // the control flow below is meant to ensure that
            // no more than 100 operations are added to the batch operation
            // since this is the maximum.
            while (delCount == 0)
            {
                delCount++;
                exerciseExists = false;
                foreach (ExerciseStorage.ExerciseEntity e in await exerS.exerciseTable.ExecuteQuerySegmentedAsync(new TableQuery<ExerciseStorage.ExerciseEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, uname)), null))
                {
                    exerciseExists = true;
                    if (++delCount == 100)
                    {
                        delCount = 0;
                        await exerS.exerciseTable.ExecuteBatchAsync(batchOp);
                        batchOp = new TableBatchOperation();
                        break;
                    }
                    batchOp.Delete(e);
                }
                if (delCount > 0 && exerciseExists)
                {
                    await exerS.exerciseTable.ExecuteBatchAsync(batchOp);
                }
            }
        }

        /**
         * Checks if the given password is the correct password. If it isn't, raises
         * WrongPasswordException
         *
         * Usage example: See main method
         */
        public async Task userPasswordQueryAsync(string uname, string p)
        {
            if (!UserStorage.UserProfileEntity.ComputeSha256Hash(p).Equals((await getUserProfileEntity(uname)).passhash)) throw new WrongPasswordException("Wrong password!");
        }


        public async Task<Stream> getUserImage(string uname, bool profile)
        {
            Stream imageStream = new MemoryStream();
            await getUserImageContainer((await getUserProfileData(uname)).group, profile).GetBlockBlobReference(uname).DownloadToStreamAsync(imageStream);
            imageStream.Position = 0;
            return imageStream;
        }

        public async Task updateUserImage(string uname, Stream imageStream, bool profile)
        {
            await getUserImageContainer((await getUserProfileData(uname)).group, profile).GetBlockBlobReference(uname).UploadFromStreamAsync(imageStream);
        }


        /**
         * Get the user name of the coach that trains the given training group
         * @precond groupName is a name of an existing training group
         */
        public async Task<string> getGroupCoach(string groupName)
        {
            return (await getGroupEntity(groupName))?.coach;
        }

        /** get a list of the groups trained by @param coachName */
        public async Task<List<string>> getGroupsOfCoach(string coachName)
        {
            List<string> res = new List<string>();
            // traverse all groups whose coach is the given coach
            foreach (UserStorage.GroupEntity group in await userS.groupsTable.ExecuteQuerySegmentedAsync(new TableQuery<UserStorage.GroupEntity>().Where(TableQuery.GenerateFilterCondition("coach", QueryComparisons.Equal, coachName)), null))
            {
                res.Add(group.RowKey);
            }
            return res;
        }

        /** Get list of names of all training groups on database that contain the given string */
        public async Task<List<string>> getGroups(string substring)
        {
            List<string> res = new List<string>();
            res.Add(UserProfileData.DEFAULT_GROUP);
            foreach (UserStorage.GroupEntity group in await userS.groupsTable.ExecuteQuerySegmentedAsync(new TableQuery<UserStorage.GroupEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, "")), null))
            {
                if (group.RowKey.Contains(substring) && !UserProfileData.DEFAULT_GROUP.Equals(group.RowKey)) res.Add(group.RowKey);
            }
            return res;
        }


        public async Task<bool> ExerciseAlreadyExists(Exercise e)
        {
            foreach (ExerciseStorage.ExerciseEntity e1 in await exerS.exerciseTable.ExecuteQuerySegmentedAsync(new TableQuery<ExerciseStorage.ExerciseEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, e.uname)), null))
            {
                if (e1.GetStartTime() == e.startTime) return true;
            }
            return false;
        }


        /** Add an exercise */
        public async Task addExerciseAsync(Exercise e)
        {
            if (!await ExerciseAlreadyExists(e))
            {
                await exerS.exerciseTable.ExecuteAsync(TableOperation.InsertOrReplace(new ExerciseStorage.ExerciseEntity(e)));
                await updateUserLastTrainingDateAsync(e.uname, e.startTime);
            }
        }

        /**
         * Add many exercises in one operation. 
         * max amount of exercises: 100.
         * Efficient usage of this method: the given exercises belongs to users from a few
         * training groups relative to total amount of given exercises.
         */
        public async Task addExercisesAsync(Exercise[] exers)
        {
            TableBatchOperation batchOp;
            int i = 0;
            bool nonEmptyBatch;
            HashSet<string> partitions = new HashSet<string>();
            string[] groups = new string[exers.Length];
            for (; i < exers.Length; i++)
            {
                groups[i] = (await getUserProfileData(exers[i].uname)).group;
                partitions.Add(groups[i]);
            }
            foreach (string partition in partitions)
            {
                batchOp = new TableBatchOperation();
                nonEmptyBatch = false;
                for (i = 0; i < exers.Length; i++)
                {
                    if (partition.Equals(groups[i]) && !await ExerciseAlreadyExists(exers[i]))
                    {
                        nonEmptyBatch = true;
                        batchOp.InsertOrReplace(new ExerciseStorage.ExerciseEntity(exers[i]));
                        await updateUserLastTrainingDateAsync(exers[i].uname, exers[i].startTime);
                    }
                }
                if (nonEmptyBatch) await exerS.exerciseTable.ExecuteBatchAsync(batchOp);
            }
        }
        public class ExercisesCompare : IComparer<Exercise>
        {
            private static readonly ExercisesCompare ex = new ExercisesCompare();
            public static ExercisesCompare get()
            {
                return ex;
            }

            public int Compare(Exercise x, Exercise y)
            {
                return x.startTime.CompareTo(y.startTime);
            }
        }

        /** Returns a list of Exercise objects that matches exercises that the given user name has done since the given date */
        public async Task<List<Exercise>> getUserExercises(string uname, DateTime since)
        {
            List<Exercise> res = new List<Exercise>();
            foreach (ExerciseStorage.ExerciseEntity e in await exerS.exerciseTable.ExecuteQuerySegmentedAsync(new TableQuery<ExerciseStorage.ExerciseEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, uname)), null))
            {
                if (e.GetStartTime() >= since) res.Add(e.generateExercise());
            }
            ExercisesCompare ex = ExercisesCompare.get();
            res.Sort(ex);
            return res;
        }

        /** Returns a list of Exercise objects, at most one per user, that matches exercises that a user in the given group has done since the given date */
        public async Task<List<Exercise>> getGroupExercises(string group, DateTime since)
        {
            List<Exercise> res = new List<Exercise>();
            // traverse users who are in the given group
            foreach (UserStorage.UserProfileEntity user in await userS.profileTable.ExecuteQuerySegmentedAsync(new TableQuery<UserStorage.UserProfileEntity>().Where(TableQuery.GenerateFilterCondition("group", QueryComparisons.Equal, group)), null))
            {
                // traverse their exercises 
                foreach (ExerciseStorage.ExerciseEntity e in await exerS.exerciseTable.ExecuteQuerySegmentedAsync(new TableQuery<ExerciseStorage.ExerciseEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user.RowKey)), null))
                {
                    if (e.GetStartTime() >= since)
                    {
                        res.Add(e.generateExercise());
                        break;
                    }
                }
            }
            return res;
        }

        /** Returns a list of Exercise objects, at most one per user, that matches the latest exercise that a user in the given group has done */
        public async Task<List<Exercise>> GetGroupLatestExercises(string group)
        {
            List<Exercise> res = new List<Exercise>();
            bool trainingExists;
            // traverse users who are in the given group
            foreach (UserStorage.UserProfileEntity user in await userS.profileTable.ExecuteQuerySegmentedAsync(new TableQuery<UserStorage.UserProfileEntity>().Where(TableQuery.GenerateFilterCondition("group", QueryComparisons.Equal, group)), null))
            {
                // traverse their exercises 
                trainingExists = false;
                ExerciseStorage.ExerciseEntity currExer = new ExerciseStorage.ExerciseEntity();
                foreach (ExerciseStorage.ExerciseEntity e in await exerS.exerciseTable.ExecuteQuerySegmentedAsync(new TableQuery<ExerciseStorage.ExerciseEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user.RowKey)), null))
                {
                    if (e.GetStartTime() >= currExer.GetStartTime())
                    {
                        currExer = e;
                        trainingExists = true;
                    }
                }
                if (trainingExists) res.Add(currExer.generateExercise());
            }
            return res;
        }




        public async Task<Poomsae> getPoomsaeForUser(string uname)
        {
            return Poomsae.GetPoomsaeForRank((Rank)(await getUserProfileEntity(uname)).rank);
        }

        public async Task<Stream> getPoomsaeImage(Poomsae p)
        {
            Stream imageStream = new MemoryStream();
            await poomS.poomsaesContainer.GetBlockBlobReference(p.ToString()).DownloadToStreamAsync(imageStream);
            imageStream.Position = 0;
            return imageStream;
        }







        public async Task WriteMessage(Message msg)
        {
            await userS.msgsTable.ExecuteAsync(TableOperation.InsertOrReplace(new UserStorage.MessageEntity(msg)));
        }


        public async Task<List<Message>> GetUserMessages(string uname)
        {
            return await GetGroupMessages((await getUserProfileEntity(uname)).group);
        }

        public async Task<List<Message>> GetGroupMessages(string groupName)
        {
            List<Message> res = new List<Message>();
            foreach (UserStorage.MessageEntity msgEnti in await userS.msgsTable.ExecuteQuerySegmentedAsync(new TableQuery<UserStorage.MessageEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, groupName)), null))
            {
                res.Add(msgEnti.generateMessage());
            }
            return res;
        }





        private static async Task initPoomsaeStorage()
        {
            CloudStorageHandler hand = new CloudStorageHandler();
            foreach (Poomsae p in Poomsae.Values)
            {
                await hand.poomS.poomsaesContainer.GetBlockBlobReference(p.ToString()).UploadFromFileAsync(string.Format("C://Users//DV//Downloads//taegeukformsjessicamandelalexman//TAEGEUK FORMS, Jessica Mandel & Alex Man_{0}.jpg", p.ToString()));
            }

        }

        /** for testing */
        public static Stream pathToStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }


        static void Main(string[] args)
        {
            //initPoomsaeStorage().GetAwaiter().GetResult();
            MainAsync().GetAwaiter().GetResult();
        }

        /** Usage example and testing */
        static async Task MainAsync()
        {
            // IMPORTANT:
            // * consult me
            // * use await before EVERY call

            //both:
            CloudStorageHandler hand = new CloudStorageHandler();

            // Bar
            // ----------------------------------------------

            // IMPORTANT:
            // * you are supposed to know and work with the UserStorage.UserProfileData class (TODO: move to Models)
            // * the '-' charachter is not allowed!! uname, fullname, coachName, group and password
            //   may NOT contain it.
            // * usernames MUST start with a letter.
            // * to use cloud storage with this module, you should have access to this variable
            //   at all times:

            string uname = "barbarosa";

            await hand.addUserAsync(new UserProfileData(UserProfileData.DEFAULT_COACH, UserProfileData.DEFAULT_COACH, Rank.Black8, true, UserProfileData.DEFAULT_GROUP), "unthinkable", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"), pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), UserProfileData.DEFAULT_GROUP);


            // Create an account (register a new user)
            UserProfileData updCoach = new UserProfileData("david", "myuname", Rank.Black8, true, UserProfileData.DEFAULT_GROUP);
            await hand.addUserAsync(updCoach, "insecure", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"), pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), "tau dragons");


            // Create an account (register a new user)
            UserProfileData upd = new UserProfileData("bar rouso", uname, Rank.Black8, false, "tau dragons");
            await hand.addUserAsync(upd, "mikMak3", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"), pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"));

            await hand.addUserAsync(new UserProfileData("heHasNoFriendsName", "heHasNoFriends", Rank.Blue, false, UserProfileData.DEFAULT_GROUP), "easy", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"), pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"));



            Message msg1 = new Message("good news", "tau dragons", "This is team Taekwondo It\nWelcome!", DateTime.Now);
            await hand.WriteMessage(msg1);

            foreach (Message msg in await hand.GetUserMessages(uname))
            {
                Debug.Assert(msg.Title.Equals(msg1.Title));
                Debug.Assert(msg.Content.Equals(msg1.Content));
            }

            foreach (Message msg in await hand.GetGroupMessages("tau dragons"))
            {
                Debug.Assert(msg.Title.Equals(msg1.Title));
                Debug.Assert(msg.Content.Equals(msg1.Content));
            }

            bool noMsgs = true;
            foreach (Message msg in await hand.GetUserMessages("heHasNoFriends"))
            {
                noMsgs = false;
            }
            Debug.Assert(noMsgs);



            if (await hand.getGroupCoach("group batman") == null)
            {
                await hand.createTrainingGroup("group batman", "myuname");
            }
            else
            {
                Console.WriteLine("group already exists...");
            }

            Debug.Assert(await hand.getGroupCoach("group batman") == "myuname");

            string[] groups = (await hand.getGroupsOfCoach("myuname")).ToArray();
            Debug.Assert(groups.Length == 2);
            Debug.Assert(groups[0].Equals("group batman") || groups[1].Equals("group batman"));
            Debug.Assert(groups[0].Equals("tau dragons") || groups[1].Equals("tau dragons"));


            Console.WriteLine(await hand.getGroupCoach("tau dragons"));

            foreach (string groupName in await hand.getGroups("t"))
            {
                Console.WriteLine(groupName);
            }
            Console.Write("just ");
            foreach (string groupName in await hand.getGroups("ta d"))   // there should be no groups here
            {
                Console.WriteLine(groupName);
            }
            Console.WriteLine("checking");


            await hand.updateUserRankAsync(uname, Rank.Blue);
            Console.WriteLine(await hand.getPoomsaeForUser(uname));


            // this is how you get a user image (it will be downloaded to the given path).
            // true is for profile image.
            // false is for backgorund image.
            FileStream f1 = File.OpenWrite(@"C:\Users\DV\Desktop\mynewimage.jpg");
            FileStream f2 = File.OpenWrite(@"C:\Users\DV\Desktop\mynewimage2.jpg");
            (await hand.getUserImage(uname, true)).CopyTo(f1);
            (await hand.getUserImage(uname, false)).CopyTo(f2);
            f1.Close();
            f2.Close();
            // this is how you get a user's rank
            Debug.Assert(Rank.Blue == (await hand.getUserProfileData(uname)).rank);
            Console.WriteLine(await hand.getPoomsaeForUser(uname));
            Console.ReadKey();

            await hand.updateUserImage(uname, pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Koala.jpg"), true);
            await hand.updateUserImage(uname, pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Desert.jpg"), false);

            f1 = File.OpenWrite(@"C:\Users\DV\Desktop\mynewimage.jpg");
            f2 = File.OpenWrite(@"C:\Users\DV\Desktop\mynewimage2.jpg");
            (await hand.getUserImage(uname, true)).CopyTo(f1);
            (await hand.getUserImage(uname, false)).CopyTo(f2);
            f1.Close();
            f2.Close();

            await hand.updateUserRankAsync(uname, Rank.Beginner);
            Debug.Assert(Rank.Beginner == (await hand.getUserProfileData(uname)).rank);
            Console.WriteLine(await hand.getPoomsaeForUser(uname));
            Console.ReadKey();



            Debug.Assert(0 == (await hand.getUserProfileData(uname)).xp);
            await hand.incrUserXPAsync(uname, 5);
            Debug.Assert(5 == (await hand.getUserProfileData(uname)).xp);


            Debug.Assert(await hand.isUsernameTaken(uname));
            Debug.Assert(!await hand.isUsernameTaken("poter harry"));


            // this try-catch block is how you verify a given password for a user

            try
            {
                // MUST USE await!!!! or else, the validation may succeed regardless of the password!
                await hand.userPasswordQueryAsync(uname, "mikmak3"); // attacker enters some wrong password
                Console.WriteLine("User validated.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine("user validation failed: {0}", e.Message);
            }

            Console.WriteLine("fullname: " + (await hand.getUserProfileData(uname)).fullname);
            Console.WriteLine("xp: " + (await hand.getUserProfileData(uname)).xp);
            Console.WriteLine("LTD: " + (await hand.getUserProfileData(uname)).lastTrainingDate);
            await hand.updateUserLastTrainingDateAsync(uname, new DateTime(2000, 2, 20));
            Console.WriteLine("LTD: " + (await hand.getUserProfileData(uname)).lastTrainingDate);
            Console.WriteLine("group: " + (await hand.getUserProfileData(uname)).group);
            Console.WriteLine("coach: " + await hand.getUserCoach(uname));


            // now a succesful example

            try
            {
                // MUST USE await!!!! or else, the validation may succeed regardless of the password!
                await hand.userPasswordQueryAsync(uname, "mikMak3");
                Console.WriteLine("User validated.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine("user validation failed: {0}", e.Message);
            }

            await hand.updateUserPasswordAsync(uname, "eHealthR0cks");

            try
            {
                // MUST USE await!!!! or else, the validation may succeed regardless of the password!
                await hand.userPasswordQueryAsync(uname, "eHeaLthR0cks");
                Console.WriteLine("User validated.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine("user validation failed: {0}", e.Message);
            }

            try
            {
                // MUST USE await!!!! or else, the validation may succeed regardless of the password!
                await hand.userPasswordQueryAsync(uname, "mikMak3");
                Console.WriteLine("User validated.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine("user validation failed: {0}", e.Message);
            }

            try
            {
                // MUST USE await!!!! or else, the validation may succeed regardless of the password!
                await hand.userPasswordQueryAsync(uname, "eHealthR0cks");
                Console.WriteLine("User validated.");
            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine("user validation failed: {0}", e.Message);
            }


            Console.ReadKey();

            // see how to remove a user below - in the end of this method



            // ----------------------------------------------
            // Roey
            // ----------------------------------------------

            // IMPORTANT:
            // * you are supposed to know and work with the Exercise class (TODO: move to Models)


            await hand.addExerciseAsync(new Exercise(new DateTime(), uname, 12.3, 13.4, 123, "push ups"));

            await hand.addExercisesAsync(new Exercise[] { new Exercise(new DateTime(2005, 7, 19), uname, 13.9, 1.4, 123, "jacknives"), new Exercise(new DateTime(2019, 12, 6), uname, 1, 13.4, 53, "pull ups"), new Exercise(new DateTime(2018, 3, 4), "myuname", 11.3, 43.4, 123, "push ups"), new Exercise(new DateTime(2016, 1, 5), uname, 15, 90, 72, "sit ups") });
            await hand.addExercisesAsync(new Exercise[] { new Exercise(new DateTime(2005, 7, 19), uname, 13.9, 1.4, 123, "jacknives"), new Exercise(new DateTime(2019, 12, 6), uname, 1, 13.4, 53, "pull ups"), new Exercise(new DateTime(2018, 3, 4), "myuname", 11.3, 43.4, 123, "push ups"), new Exercise(new DateTime(2016, 1, 5), uname, 15, 90, 72, "sit ups") });

            Console.WriteLine("Heads up!");

            foreach (Exercise exer in await hand.getGroupExercises("tau dragons", new DateTime()))
            {
                Console.WriteLine("{0}: user {1}, start {2}, avg heart rate {3}", exer.exerciseName, exer.uname, exer.startTime, exer.avgHR);
            }

            Console.WriteLine("Observe!");
            Console.ReadKey();

            foreach (Exercise exer in await hand.getUserExercises(uname, new DateTime()))
            {
                Console.WriteLine("{0}: start {1}, avg heart rate {2}", exer.exerciseName, exer.startTime, exer.avgHR);
            }


            Console.ReadKey();


            // ----------------------------------------------
            // Testing
            // ----------------------------------------------

            /* WORKS:
            int k = 1;
            FileStream myF;
            foreach(Poomsae p in Poomsae.Values)
            {
                myF = File.OpenWrite(string.Format(@"C:\Users\DV\Desktop\mypooms\poo{0}.jpg", k++));
                (await hand.getPoomsaeImage(p)).CopyTo(myF);
                myF.Close();
            }
            */


            // remove a user   i.e delete account
            // don't delete accounts of coaches!
            await hand.removeUserAsync(uname);

            await hand.removeUserAsync("heHasNoFriends");

            Console.WriteLine("after user {0} removed:", uname);

            foreach (Exercise exer in await hand.getGroupExercises("tau dragons", new DateTime()))
            {
                Console.WriteLine("{0}: user {1}, start {2}, avg heart rate {3}", exer.exerciseName, exer.uname, exer.startTime, exer.avgHR);
            }

            foreach (Exercise exer in await hand.getUserExercises(uname, new DateTime()))
            {
                Console.WriteLine("{0}: start {1}, avg heart rate {2}", exer.exerciseName, exer.startTime, exer.avgHR);
            }


            Console.WriteLine("Done!");
            Console.ReadKey();
        }
        public async void InitAsync()
        {
            CloudStorageHandler handler = new CloudStorageHandler();
            UserProfileData trainer1 = new UserProfileData("trainer1 self", "trainer1", Rank.Black8, true, UserProfileData.DEFAULT_GROUP);
            UserProfileData trainee11 = new UserProfileData("trainee11 self", "trainee11", Rank.Blue, false, "Train 1");
            UserProfileData trainee12 = new UserProfileData("trainee12 self", "trainee12", Rank.Blue, false, "Train 1");
            UserProfileData trainee21 = new UserProfileData("trainee21 self", "trainee21", Rank.Blue, false, "Train 2");
            UserProfileData trainee22 = new UserProfileData("trainee22 self", "trainee22", Rank.Blue, false, "Train 2");


            await handler.addUserAsync(trainer1, "trainer1", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"),
                pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"),"Train1");
            await handler.createTrainingGroup("Train2", "trainer1");
            await handler.addUserAsync(trainee11, "trainee11", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"),
    pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), "Train1");
            await handler.addUserAsync(trainee12, "trainee12", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"),
pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), "Train1");
            await handler.addUserAsync(trainee21, "trainee21", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"),
pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), "Train2");
            await handler.addUserAsync(trainee22, "trainee22", pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg"),
pathToStream(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg"), "Train2");
            await handler.addExercisesAsync(new Exercise[] {
                new Exercise(DateTime.Now.AddDays(-1), "trainee11", 70,88,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-2), "trainee11", 60,78,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-3), "trainee11", 60, 68, 4, "pull ups"),
                new Exercise(DateTime.Now.AddDays(-4), "trainee11", 50, 68, 4, "pull ups")
            });
            await handler.addExercisesAsync(new Exercise[] {
                new Exercise(DateTime.Now.AddDays(-1), "trainee12", 70,88,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-2), "trainee12", 60,78,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-3), "trainee12", 60, 68, 4, "pull ups"),
                new Exercise(DateTime.Now.AddDays(-4), "trainee12", 50, 68, 4, "pull ups")
            });
            await handler.addExercisesAsync(new Exercise[] {
                new Exercise(DateTime.Now.AddDays(-1), "trainee21", 70,88,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-2), "trainee21", 60,78,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-3), "trainee21", 60, 68, 4, "pull ups"),
                new Exercise(DateTime.Now.AddDays(-4), "trainee21", 50, 68, 4, "pull ups")
            });
            await handler.addExercisesAsync(new Exercise[] {
                new Exercise(DateTime.Now.AddDays(-1), "trainee22", 70,88,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-2), "trainee22", 60,78,4,"pull ups"),
                new Exercise(DateTime.Now.AddDays(-3), "trainee22", 60, 68, 4, "pull ups"),
                new Exercise(DateTime.Now.AddDays(-4), "trainee22", 50, 68, 4, "pull ups")
            });

        }
        internal async Task addExercisesAsync(List<Exercise> myExercises)
        {
            await addExercisesAsync(myExercises.ToArray());
        }
    }

    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(string msg) : base(msg) { }
    }
   
}
