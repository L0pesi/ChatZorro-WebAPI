using ChatZorro.Entities;
using ChatZorro.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace ChatZorro.Services
{
    internal class MessageService
    {
        private static List<User> _users = new User().Users();
        internal Message Send(Models.NewMessageModel msg)
        {
            ////1. Utilizador To existe? (From já está a ser avaliado na Authentication)
            //if (_users.SingleOrDefault(x => x.Username == msg.To && x.Inactive == false) == null)
            //{
            //    throw new Exception("Unexistent user.");
            //}

            ////2. Criar chat room do utilizador se não existir
            int fromUserCode = _users.SingleOrDefault(x => x.Username == BasicAuthenticationHandler.GetAuthorizationSession().Username).Code;
            //if (msg.ChatCode == null)
            //{
            
            //}

            Message msgSend = new Message()
            {
                Code = new Message().NewMessageCode(),
                FromCode = fromUserCode,
                ChatCode = (int)msg.ChatCode,
                Body = new Body()
                {
                    Text = msg.Text
                },
                SendDate = DateTime.Now,
            };

            //3. Envia a mensagem 
            SaveMessage(msgSend);

            return msgSend;
        }

        private void SaveMessage(Message msgSend)
        {
            ////(grava no ficheiro no profile)
            //string json = JsonSerializer.Serialize(msgSend);
            //string path = System.IO.Directory.GetCurrentDirectory() + "\\Data\\Messages";
            //string fileName = System.IO.Path.Combine(path, $"{msgSend.Code}.pmsg");
            //System.IO.File.WriteAllText(fileName, json);

            //gravar na bd
            try
            {
                DataBaseConnection dbConn = new DataBaseConnection();
                NpgsqlCommand command = null;

                command = dbConn.NewCommand($@"INSERT INTO Tbl_Message(id_message, id_user, id_groupchat, send_date, body)
                                               VALUES({msgSend.Code},{msgSend.FromCode}, {msgSend.ChatCode}, '{DateTime.Now}', '{msgSend.Body.Text}')");
                command.ExecuteNonQuery();
                command.Dispose();
                dbConn.CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static List<Messg> Sincronize()
        {
            //string path = System.IO.Directory.GetCurrentDirectory() + "\\Data";
            //string fileName = System.IO.Path.Combine(path, "message.json");
            //string json = System.IO.File.ReadAllText(fileName);
            //var msgSended = JsonSerializer.Deserialize<Models.NewMessageModel>(json);
            try
            {
                DataBaseConnection dbConn = new DataBaseConnection();
                NpgsqlCommand command = null;
                NpgsqlDataReader reader = null;
                List<Messg> msgList = new List<Messg>();

                //command = dbConn.NewCommand($@"SELECT m.id_message, m.id_user, m.id_groupchat, m.creation_date, m.send_date, body
                //                              FROM Tbl_Message m
                //                              INNER JOIN Tbl_GroupChat_User_Relationship r ON m.id_groupchat = r.id_groupchat AND r.id_user <> m.id_user
                //                              INNER JOIN Tbl_ReceivedMessages s ON m.id_message = s.id_message AND s.id_user <> r.id_user
                //                              WHERE r.id_user = {BasicAuthenticationHandler.GetAuthorizationSession().Username}");
                command = dbConn.NewCommand($@"SELECT m.id_message, m.id_user, m.id_groupchat, m.creation_date, m.send_date, body
                                              FROM Tbl_Message m
                                              INNER JOIN Tbl_GroupChat_User_Relationship r ON m.id_groupchat = r.id_groupchat AND r.id_user <> m.id_user AND r.id_user = {_users.FirstOrDefault(u => u.Username == BasicAuthenticationHandler.GetAuthorizationSession().Username).Code}");

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //      {
                    //      id: 0,
                    //sender: 2,
                    //body: "Onde andas, mossu?",
                    //time: "April 25, 2018 13:21:03",
                    //status: 2,
                    //recvId: 0,
                    //recvIsGroup: false

                    //      }
                    msgList.Add(new Messg()
                    {
                        id = reader.GetFieldValue<int>(0),
                        sender = reader.GetFieldValue<int>(2),
                        body = reader.GetFieldValue<string>(5),
                        timer = reader.GetFieldValue<DateTime>(3),
                        recvIsGroup = true
                    });
                }
                command.Dispose();

                //Falta colocar as mensagens com o estado de receção
                dbConn.CloseConnection();
                return msgList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        internal static bool Delete(int msgCode)
        {
            try
            {
                DataBaseConnection dbConn = new DataBaseConnection();
                NpgsqlCommand command = null;
                List<Messg> msgList = new List<Messg>();

                command = dbConn.NewCommand($@"DELETE FROM Tbl_ReceivedMessages
                                              WHERE id_message = (SELECT Tbl_ReceivedMessages.id_message FROM  Tbl_ReceivedMessages
                                              INNER JOIN Tbl_Message ON Tbl_ReceivedMessages.id_message  = Tbl_Message.id_message AND Tbl_Message.id_user = {_users.FirstOrDefault(u => u.Username == BasicAuthenticationHandler.GetAuthorizationSession().Username).Code}
                                            AND Tbl_ReceivedMessages.id_message = {msgCode} LIMIT 1);
                                            DELETE FROM Tbl_Message
                                            WHERE id_user = {_users.FirstOrDefault(u => u.Username == BasicAuthenticationHandler.GetAuthorizationSession().Username).Code}
                                            AND id_message = {msgCode}");

                command.ExecuteNonQuery();
                command.Dispose();
                dbConn.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
