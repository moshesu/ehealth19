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
using Android.Content;

namespace UVapp
{
    
    [Activity(Label = "UVSafe", MainLauncher = true)]
    public class Login_activity : Activity 
    {
        EditText username;
        EditText password;
        TextView txt;
        Button loginBtn, addUserBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.loginLayout);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            txt = FindViewById<TextView>(Resource.Id.logintxt);
            loginBtn.Click += LoginFunction;
            addUserBtn = FindViewById<Button>(Resource.Id.createButton);
            addUserBtn.Click += AddUserFunction;
        }

        public void LoginFunction(object sender, System.EventArgs e)
        {
            string usernameText = username.Text;
            string passwordText = password.Text;
            
            txt.Text = "Logging in...";
            
            
            User user = UserManager.GetUser(usernameText, passwordText);
            
            if (user != null)
            {
                txt.Text = "login successful";
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                mainIntent.PutExtra("userJson", user.ToString());
                MainActivity.loggedIn = true;
                StartActivity(mainIntent);
            }
            else
            {
                txt.Text = "username or password incorrect , please try again";
                password.Text = "";
                username.Text = "";
            }
              
            return;
        }

        void LoginFunctionOld()
        {
            //call cloud fuction to check if user is in database that returns a number describing a situation 

            UserManager userManager = new UserManager();
            User user = new User(username.Text, password.Text);
            int res = userManager.GetUserLoginStatus(username.Text, password.Text);

            switch (res)
            {
                case 0:
                    txt.Text = "login successful";
                    Intent mainIntent = new Intent(this, typeof(MainActivity));
                    mainIntent.PutExtra("name", user.UserName);
                    MainActivity.loggedIn = true;
                    StartActivity(mainIntent);
                    break;
                case 1:
                    txt.Text = "username or password incorrect , please try again";
                    password.Text = "";
                    username.Text = "";
                    break;
                default: break;
            }

            return;
        }
      
        public void AddUserFunction(object sender, System.EventArgs e) {
            /*  User user = new User(username.Text, password.Text);
              username.Text = "";
              password.Text = "";
              txt.Text = "You have now created an account, please try to login";   
              UserManager.createUser(user);
             // UserManager.UpdateUserExposedField(userName, Password, 5);   // exaple for updating user timeExposed
             */
            Intent registerIntent = new Intent(this, typeof(Register_activity));
            //registerIntent.PutExtra("name", username.Text);
            //MainActivity.loggedIn = true;
            StartActivity(registerIntent);
            return;
        }
    }



   

}