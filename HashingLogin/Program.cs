using static System.Console;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using HashingLogin.Database;

using HashingContext context = new HashingContext();
int loginAttempts = 0;
Prompt();


void Prompt()
{
    // A menu running the selection between registration and login.
    Clear();
    WriteLine("[R] Register [L] Login");
    while (true)
    {
        var input = ReadLine().ToUpper()[0]; // Causes a problem if no input has been made.
        switch (input)
        {
            case 'R': Register(); break;
            case 'L': Login(); break;
            default:
                break;
        }
    }
}

void Login()
{// Login process
    Clear();
    WriteLine("==Login==");
    if (loginAttempts != 0 && loginAttempts != 5) // Checks if the user has failed login attempts, and inform the user of remaining attempts.
    {
        WriteLine($"Login failed, {5 - loginAttempts} attempts left");
    }
    else if(loginAttempts == 5) // If the user has 5 failed attempts, informs the user no more attempts.
    {
        WriteLine("Too many failed attempts, login unavailable.");
    }
    if (loginAttempts != 5) // If the user has not failed 5 attempts at login, do not run the login flow.
    {
        // Recieves the users username nad password.
        Write("Username ");
        var name = ReadLine();
        Write("Password ");
        var password = ReadLine();

        var userFound = context.LoginInformation.Any(u => u.Username == name); // Asks the database if a user with the username exists.

        if (userFound) // If a user is found, run the loginflow.
        {
            var loginUser = context.LoginInformation.FirstOrDefault(u => u.Username == name); // Gets the user from the database.

            if (HashPassword($"{password}{loginUser.Salt}") == loginUser.Userpassword) // Checks if the password is correct.
            {

                var salt = DateTime.UtcNow.ToString(); // Makes a new Salt.
                var hashedPassword = HashPassword($"{password}{salt}"); // Makes the new password with salt in a hash.
                loginUser.Userpassword = hashedPassword; // Saves the new password to the user model.
                loginUser.Salt = salt; // saves the new salt to the user model.
                context.LoginInformation.Update(loginUser); // Updates the context with the new information
                context.SaveChanges(); // Applies the changes to the database.

                loginAttempts = 0; // resets the login attempts
                Clear();
                WriteLine("Logged in.");
                WriteLine(loginUser.Userpassword);
                ReadLine();
            }
            else // If the password is not correct, register the login attempt, and return to menu.
            {
                loginAttempts++;
                Prompt();

            }
        }
    }

}
void Register()
{
    // Registration process
    Clear();
    WriteLine("==Register==");
    // Recieves the username and password from the user.
    Write("Username ");
    var name = ReadLine();
    Write("Password ");
    var password = ReadLine();

    var salt = DateTime.UtcNow.ToString(); // Makes a salt.
    var hashedPassword = HashPassword($"{password}{salt}"); // Creates the hashed password with added salt.

    context.LoginInformation.Add(new HashingLogin.loginInformation() { Username = name, Userpassword = hashedPassword, Salt = salt }); // Adds the user to the context
    context.SaveChanges(); // Saves the context with the added user.

    while (true)
    { // Runs a while-loop to listen for user key input.
        Clear();
        WriteLine("Registrering færdig");
        WriteLine("[B] Tilbage");
        if (ReadKey().Key == ConsoleKey.B) // If the user presses the key, return to menu.
        {
            Prompt();
        }
    }
}
string HashPassword(string password)
{// Hashes the password
    SHA256 hash = SHA256.Create();
    var passwordBytes = Encoding.Default.GetBytes(password); // converts the recieved password to bytes.
    var hashedPassword = hash.ComputeHash(passwordBytes); // Hashes the password

    return Convert.ToHexString(hashedPassword); // returns the Hex string of the hashed password
}