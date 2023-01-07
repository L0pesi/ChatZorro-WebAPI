using ChatZorro.Entities;
using ChatZorro.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChatZorro.Services
{
    internal class ChatService
    {
        //Verify if chat exists
        //If not exist create

        internal List<Chat> Chats()
        {
            //Conecção à BD
            DataBaseConnection dbConn = new DataBaseConnection();
            NpgsqlCommand command = null;
            NpgsqlDataReader reader = null;
            List<Chat> chats = new List<Chat>();

            command = dbConn.NewCommand($@"SELECT id_groupchat, name FROM Tbl_GroupChat");
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                chats.Add(new Chat()
                {
                    Info = new InfoChat()
                    {
                        Code = reader.GetFieldValue<int>(0),
                        Name = reader.GetFieldValue<string>(1)
                    },
                    Users = GetUsersChat(dbConn, reader.GetFieldValue<int>(0))
                });
            }
            command.Dispose();
            reader.Close();
            dbConn.CloseConnection();
            return chats;
        }

        private List<UserChat> GetUsersChat(DataBaseConnection dbConn, int chatCode)
        {
            List<UserChat> users = null;
            NpgsqlDataReader reader = dbConn.NewCommand($@"SELECT id_groupchat, id_user, admin FROM Tbl_GroupChat_User_Relationship WHERE id_groupchat = {chatCode}").ExecuteReader();

            List<User> usersList = new User().Users();
            while (reader.Read())
            {
                if (users == null)
                    users = new List<UserChat>();

                var user = usersList.Find(x => x.Code == reader.GetFieldValue<int>(1));
                users.Add(new UserChat()
                {
                    Code = user.Code,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Password = user.Password,
                    Inactive = user.Inactive,
                    Admin = reader.GetFieldValue<bool>(2)
                });
            }
            reader.Close();
            return users;
        }
    
    }
}
