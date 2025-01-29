using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckInSystem.Models;

namespace CheckInSystem.Tests
{
    public class AdminUserTest
    {
        [Fact]
        public void Admin_Create_Delete_Update_Getlist()
        {
            string TestUserName = "TestUser";
            string TestPassWord = "TestPass";

            // Step 1: Create user
            AdminUser.CreateUser(TestUserName, TestPassWord);

            // Step 2: Verify user is created by checking if the user can be found in the list of admin users
            var users = AdminUser.GetAdminUsers();
            var createdUser = users.FirstOrDefault(u => u.Username == TestUserName);
            Assert.NotNull(createdUser);

            // Step 3: Login with created user
            var loggedInUser = AdminUser.Login(TestUserName, TestPassWord);
            Assert.NotNull(loggedInUser);

            // Step 4: Delete user by ID
            createdUser?.Delete(createdUser.ID);

            // Step 5: Verify user is deleted (i.e., not present in the list anymore)
            var usersAfterDeletion = AdminUser.GetAdminUsers();
            var deletedUser = usersAfterDeletion.FirstOrDefault(u => u.Username == TestUserName);
            Assert.Null(deletedUser);
        }
    }
}
