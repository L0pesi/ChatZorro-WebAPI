using ChatZorro.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace ChatZorro.Entities
{
    public class User : Services.Configs
    {
        public User()
        {
            base.Load();
        }

        public int Code { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public byte[] Photo { get; set; }
        public bool Inactive { get; set; }

        internal List<InfoChat> GetRoomsUser(DataBaseConnection dbConn, int username)
        {
            List<InfoChat> rooms = null;
            NpgsqlDataReader reader = null;
            NpgsqlCommand command = dbConn.NewCommand(@$"SELECT Tbl_GroupChat.id_groupchat, name FROM Tbl_GroupChat
                                                      INNER JOIN Tbl_GroupChat_User_Relationship ON Tbl_GroupChat.id_groupchat = Tbl_GroupChat_User_Relationship.id_groupchat
                                                      WHERE id_user = '{username}'");
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (rooms == null)
                    rooms = new List<InfoChat>();

                rooms.Add(new InfoChat()
                {
                    id = reader.GetFieldValue<int>(0),
                    name = reader.GetFieldValue<string>(1),
                    members = GetUsersChat(reader.GetFieldValue<int>(0), username),
                    pic = ""
                });
            }
            dbConn.CloseConnection();
            return rooms;
        }

        private List<int> GetUsersChat(int chatCode, int userLogin)
        {
            List<int> members = null;
            NpgsqlDataReader reader = new DataBaseConnection().NewCommand($@"SELECT id_groupchat, id_user, admin FROM Tbl_GroupChat_User_Relationship WHERE id_groupchat = {chatCode} AND id_user <> {userLogin}").ExecuteReader();

            //List<User> usersList = new User().Users();
            while (reader.Read())
            {
                if (members == null)
                    members = new List<int>();

                members.Add(reader.GetFieldValue<int>(1));
            }
            reader.Close();
            return members;
        }

        internal List<User> Users()
        {
            //Conecção à BD
            DataBaseConnection dbConn = new DataBaseConnection();
            NpgsqlCommand command = null;
            NpgsqlDataReader reader = null;
            List<User> users = new List<User>();

            command = dbConn.NewCommand($"SELECT id_user, firstname, lastname, username, password, inactive FROM Tbl_User");
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User()
                {
                    Code = reader.GetFieldValue<int>(0),
                    FirstName = reader.GetFieldValue<string>(1),
                    LastName = reader.GetFieldValue<string>(2),
                    Username = reader.GetFieldValue<string>(3),
                    Password = reader.GetFieldValue<string>(4),
                    Inactive = reader.GetFieldValue<bool>(5),
                });
            }
            command.Dispose();
            reader.Close();
            dbConn.CloseConnection();
            return users;
        }

        internal List<Contact> GetContactList()
        {
            var users = new User().Users();
            List<Contact> contacts = new List<Contact>();
            foreach (var contact in users)
            {
                contacts.Add(new Contact()
                {
                    id = contact.Code,
                    lastSeen = DateTime.Now,
                    name = contact.FirstName + " " + contact.LastName,
                    number = contact.Username,
                    pic = ""
                });
            }
            return contacts;
        }
    }

    public class Profile
    {
        [Required]
        public User User { get; set; }
        public List<InfoChat> Rooms { get; set; }
        public List<Contact> ContactList { get; set; }
    }

    public class Contact
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string pic { get; set; }
        public DateTime lastSeen { get; set; }
    }
}
