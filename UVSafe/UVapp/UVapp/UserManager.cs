using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Threading.Tasks;

// ADD THIS PART TO YOUR CODE
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;


namespace UVapp
{
    class UserManager
    {
        // ADD THIS PART TO YOUR CODE
        private const string EndpointUrl = "https://uvsafe2.documents.azure.com:443/";
        private const string PrimaryKey = "Oc2HgAOWqt71ykwIVN4lOtsjYCVDQXxBuXEzXWqUxRBy42v9NNKD1cziNPyu5YBBzHla8JE1UDvn7PcZQlcEjg==";
        static private DocumentClient client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey); 
        static private string databaseName = "UsersDB";
        static private string collectionName = "UsersCollection";
        enum LoginStatus {Logged , NotLogged };
        //TBD: 7 days of the week exposete , Last day recorded, SkinType, maxUv
        
        
        //TBD: create Regestartion page, moced right into mainActivity afterwards


        public async static void CreateDB()
        {
             await UserManager.client.CreateDatabaseIfNotExistsAsync(new Database { Id = UserManager.databaseName });
        }
        public async static void CreateCollection()
        {
            await UserManager.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(UserManager.databaseName), new DocumentCollection { Id = UserManager.collectionName });
        }

        public async static void UpdateUserExposedField(User user , int exposedMinutes)
        {
            user.TimeExposed = exposedMinutes;
            await UserManager.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, user.Id), user);

        }

        public async static Task UpdateUser(User user)
        {
            user.Date = User.getTodayDateString();
            
            await UserManager.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, user.Id), user);
        }

        public async static void createUser(User user)
        {
         
            await UserManager.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(UserManager.databaseName, UserManager.collectionName), user);

        }


        public static User GetUser(string userName, string password)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<User> userQuery = UserManager.client.CreateDocumentQuery<User>(
                    UriFactory.CreateDocumentCollectionUri(UserManager.databaseName, UserManager.collectionName), queryOptions)
                    .Where(f => (f.UserName == userName && f.Password == password));

            Console.WriteLine("Running direct SQL query...");
            foreach (User user in userQuery)
            {
                Console.WriteLine("\tRead and found user {0}", user);
                return user;
            }
            
            Console.WriteLine("we did nor found user!");

            return null;
        }
    
        public int GetUserLoginStatus(string userName, string password)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<User> userQuery = UserManager.client.CreateDocumentQuery<User>(
                    UriFactory.CreateDocumentCollectionUri(UserManager.databaseName, UserManager.collectionName), queryOptions)
                    .Where(f => (f.UserName == userName && f.Password == password));

            Console.WriteLine("Running direct SQL query...");
            foreach (User user in userQuery)
            {
                Console.WriteLine("\tRead and found user {0}", user);
                return (int)LoginStatus.Logged;
            }

            Console.WriteLine("we did nor found user!");

            return (int)LoginStatus.NotLogged;
        }


    }
}
