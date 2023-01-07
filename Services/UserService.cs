using ChatZorro.Entities;
using ChatZorro.Helpers;
using ChatZorro.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatZorro.Services
{
    public interface IUserService
    {
        Task<Profile> Authenticate(string username, string password);
        Task<UserModel> Registration(UserModel model);
        Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        private List<User> _users = new User().Users();
        private Profile ValidationUserAcount(Models.AuthenticateModel credentials)
        {
            User user = _users.FirstOrDefault<User>(u => u.Username == credentials.Username);
            if(user != null)
            {
                //1.Valida se conta está ativa
                if(user.Inactive)
                {
                    throw new Exception("Inactive acount.");
                }

                //2. Validar na BD se password é correta
                if (user.Password != credentials.Password)
                {
                    throw new Exception("Invalid password.");
                }
            }
            else
            {
                throw new Exception("Unregistred user.");
            }

            //3. Devolver dados do utilizador
            var profile = new Profile()
            {
                User = user.WithoutPassword(),
                Rooms = user.GetRoomsUser(new DataBaseConnection(), user.Code),
                ContactList = user.GetContactList()
            };
            return profile;
        }

        

        /// <summary>
        /// Valida autenticação e devolve o perfil do utilizador
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Profile> Authenticate(string username, string password)
        {
            try
            {
                Profile profile = ValidationUserAcount(new Models.AuthenticateModel() { Username = username, Password = password });
                //var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

                // return null if user not found
                if (profile == null)
                    return null;

                return profile;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserModel> Registration(UserModel model)
        {
            try
            {
                DataBaseConnection dbConn = new DataBaseConnection();
                NpgsqlCommand command = null;

                var userexist = _users.FirstOrDefault<User>(x => x.Username == model.Username);
                if (userexist != null)
                    throw new Exception("User has exists.");

                command = dbConn.NewCommand($@"INSERT INTO Tbl_User(firstname, lastname, username, password, inactive)
                                    VALUES('{model.FirstName}','{model.LastName}','{model.Username}','{model.Password}', 'False')");
                command.ExecuteNonQuery();
                command.Dispose();
                dbConn.CloseConnection();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _users.WithoutPasswords());
        }
    }
}