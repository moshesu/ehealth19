using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
namespace SignBuzz
{
    public class MainUserManager
    {
        static MainUserManager defaultInstance = new MainUserManager();
        MobileServiceClient client;
        IMobileServiceTable<User_compete> user_CompeteTable;
        IMobileServiceTable<User> userTable;
        IMobileServiceTable<User_game> user_gameTable;
        IMobileServiceTable<User_game2> user_game2Table;
        IMobileServiceTable<User_game3> user_game3Table;

        private MainUserManager()
        {
            this.client = new MobileServiceClient("https://signbuzz.azurewebsites.net");
            this.userTable = client.GetTable<User>();
            this.user_game2Table = client.GetTable<User_game2>();
            this.user_game3Table = client.GetTable<User_game3>();
            this.user_gameTable = client.GetTable<User_game>();
            this.user_CompeteTable= client.GetTable<User_compete>();

        }

        public static MainUserManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }
        public IMobileServiceTable<User> CurrentUserTable
        {
            get { return userTable; }
        }
        public IMobileServiceTable<User_compete> CurrentUser_CompeteTable
        {
            get { return user_CompeteTable; }
        }
        public async Task SaveUserCompeteAsync(User_compete user_game)
        {
            try
            {
                await user_CompeteTable.InsertAsync(user_game);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task UpdateUserCompeteAsync(User_compete user_game)
        {
            try
            {
                await user_CompeteTable.UpdateAsync(user_game);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public IMobileServiceTable<User_game> CurrentUser_GameTable
        {
            get { return user_gameTable; }
        }
        public async Task SaveUserGameAsync(User_game user_game)
        {
            try
            {
                await user_gameTable.InsertAsync(user_game);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task UpdateUserGameAsync(User_game user_game)
        {
            try
            {
                await user_gameTable.UpdateAsync(user_game);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public IMobileServiceTable<User_game2> CurrentUser_Game2Table
        {
            get { return user_game2Table; }
        }
        public IMobileServiceTable<User_game3> CurrentUser_Game3Table
        {
            get { return user_game3Table; }
        }
        public async Task SaveUserGame3Async(User_game3 user_game3)
        {
            try
            {
                await user_game3Table.InsertAsync(user_game3);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task UpdateUserGame3Async(User_game3 user_game3)
        {
            try
            {
                await user_game3Table.UpdateAsync(user_game3);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task SaveUserGame2Async(User_game2 user_game2)
        {
            try
            {
                await user_game2Table.InsertAsync(user_game2);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task UpdateUserGame2Async(User_game2 user_game2)
        {
            try
            {
                await user_game2Table.UpdateAsync(user_game2);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task SaveUserAsync(User user)
        {
            try
            {
              await userTable.InsertAsync(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task UpdateUserAsync(User user)
        {
            try
            {
                await userTable.UpdateAsync(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
    }
}
