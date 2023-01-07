using ChatZorro.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace ChatZorro.Entities
{
    internal class Chat : Services.Configs
    {
        internal Chat()
        {
            Users = new List<UserChat>();
            base.Load();
        }
        public InfoChat Info { get; set; }
        public byte[] Photo { get; set; }
        [Required]
        public List<UserChat> Users { get; set; }
        //public List<int> MessageCodes { get; set; }
        //public List<Message> MessageCodes { get; set; }

        internal int CreateRoom(int userFrom, int userTo)
        {
            try
            {
                DataBaseConnection dbConn = new DataBaseConnection();
                NpgsqlCommand command = null;

                int chatCode = base.NewChatCode();
                command = dbConn.NewCommand($@"INSERT INTO Tbl_GroupChat(id_groupchat, name)
                                            VALUES({chatCode}, 'Username');
                                            INSERT INTO Tbl_GroupChat_User_Relationship(id_groupchat, id_user, admin)
                                            VALUES({chatCode}, {userFrom}, 'True');
                                            INSERT INTO Tbl_GroupChat_User_Relationship(id_groupchat, id_user, admin)
                                            VALUES({chatCode}, {userTo}, 'True');");
                command.ExecuteNonQuery();
                command.Dispose();
                dbConn.CloseConnection();
                return chatCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    internal class UserChat : User
    {
        public bool Admin { get; set; }
    }

    public class InfoChat
    {
        public InfoChat()
        {
            members = new List<int>();
        }
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public List<int> members { get; set; }
        public string pic { get; set; }

    }

    //private List<UserChat> GetUsersChat(DataBaseConnection dbConn, int chatCode)
    //{
    //    List<UserChat> users = null;
    //    SqlDataReader reader = dbConn.NewCommand($@"SELECT [id_groupchat], [id_user], [admin] FROM [Tbl_GroupChat_User_Relationship] WHERE [id_groupchat] = {chatCode}").ExecuteReader();

    //    List<User> usersList = new User().Users();
    //    while (reader.Read())
    //    {
    //        if (users == null)
    //            users = new List<UserChat>();

    //        var user = usersList.Find(x => x.Code == reader.GetFieldValue<int>(1));
    //        users.Add(new UserChat()
    //        {
    //            Code = user.Code,
    //            FirstName = user.FirstName,
    //            LastName = user.LastName,
    //            Username = user.Username,
    //            Password = user.Password,
    //            Inactive = user.Inactive,
    //            Admin = reader.GetFieldValue<bool>(2)
    //        });
    //    }
    //    reader.Close();
    //    return users;
    //}
}
