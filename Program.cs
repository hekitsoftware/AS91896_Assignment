using System;

namespace DTO2207
{
    public class BoxInfo
    {
        //initialising all box variables as blank
        public double BoxHeight { get; set; }
        public double BoxLength { get; set; }
        public double BoxWidth { get; set; }
        public double Volume { get; set; }
    }

    public class UserInfo
    {
        //Initialising all user variables (as requested)
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string returnCost { get; set; }
        public string SelectedIsland { get; set; } // < This variable is used in the receipt at the end

        //Initialising additional variables
        public string ItemDesc { get; set; }
        public string OrderNumber { get; set; }
        // ^ This "OrderNumber" is an added feature I think will improve the request...
        // However, as you will see later in the code, right now it is just for show/demo purposes, but could
        // later be connected to an API to handle actual order numbers for the client/company.
    }

    public class IslandsData
    {
        //setting up our island data
        public string islandName { get; set; }
        public double rateMulti { get; set; }

        public IslandsData(string name, double multi)
        {
            islandName = name;
            rateMulti = multi;
        }
    }

    class MainProgram
    {
        static BoxInfo box = new BoxInfo();
        static UserInfo user = new UserInfo();
        // ^ Creating references to our external classes

        static void Main(string[] args) // < This is the first method to be run when the Terminal application opens
        {
            user.OrderNumber = GenerateOrderNumber(); // < This is being called first, seeing as it's the least important right now, in it's demo-only form.

            IslandsData[] islandOptions = new IslandsData[3];
            islandOptions[0] = new IslandsData("North Island", 1);
            islandOptions[1] = new IslandsData("South Island", 1.5);
            islandOptions[2] = new IslandsData("Stewart Island", 2);
            // ^ An array with 2 objects per entry, which is set up to be easily added upon
            //   for the potential other islands mentioned in the breif.

            Console.WriteLine("------------------------------");
            Console.WriteLine("Welcome to the ONLINZ Return-Cost Calculator");

            Console.WriteLine("Please Enter your FIRST NAME");
            Console.WriteLine(""); // < Padding
            user.Name = Console.ReadLine().ToUpper();

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Welcome {user.Name}! Please begin by filling out some basic customer info below...");
            Console.WriteLine("");

            //Call the method to get the user's basic shipping info
            getUserInfo();

            // Call the method to get the package dimensions
            GetBoxDimensions();

            // Call the method to calculate the return cost for the user
            CalculateReturnCost(islandOptions);

            // User Send-Off + Receipt printing
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Thank you {user.Name} for using ONLINZ for your package returning needs.");
            Console.WriteLine("Here is your recepit:");

            // Call the method to print the recepit
            Console.WriteLine(""); // < Padding
            PrintReceipt();
            Console.WriteLine(""); // < Padding

            Console.WriteLine("Would you like to calculate the return cost for another item? (Y/N)");
            Console.WriteLine(""); // < Padding
            string continueChoice = Console.ReadLine().ToUpper();
            if (continueChoice == "Y" || continueChoice == "YES")
            {
                // Restart the process if the user chooses to calculate again
                Console.Clear();
                Main(args);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                Console.WriteLine("Thank you for using the ONLINZ Return-Cost Calculator! \n \n Press ANY KEY to close the program");
                // Exit the program
                Console.ReadKey();
            }

        }
        
        static string GenerateOrderNumber()
        {
            Random random = new Random();
            int randomNum = random.Next(1000, 9999);  // Random number between 1000 and 9999
            return "ORD-NZ-" + randomNum.ToString();
        }

        static void getUserInfo()
        {
            // Ask for the user's info...
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Welcome {user.Name}! Please begin by filling out some basic customer info below...");
            Console.WriteLine("---");
            Console.WriteLine(""); // < Padding
            Console.WriteLine("Please enter your SURNAME:");
            // < Padding
            user.Surname = Console.ReadLine().ToUpper();
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Welcome {user.Name}! Please begin by filling out some basic customer info below...");
            Console.WriteLine("---");
            Console.WriteLine(""); // < Padding
            Console.WriteLine("Please enter your HOME/SHIPPING ADDRESS:");
            Console.WriteLine(""); // < Padding
            user.Address = Console.ReadLine().ToUpper();
            Console.Clear();
            user.PhoneNumber = GetValidPhoneNumber();
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Welcome {user.Name}! Please begin by filling out some basic customer info below...");
            Console.WriteLine("---");
            Console.WriteLine(""); // < Padding
            Console.WriteLine("Please breifly describe the ITEM you wish to return in 6 words or less:");
            Console.WriteLine(""); // < Padding
            user.ItemDesc = Console.ReadLine().ToUpper();

            // Print a copy of the entered data to check if the user has entered their data correctly...
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Thank you for entering your information... Does this look right? \n(Type 'Y' for Yes, or 'N' to re-enter your details)");
            Console.WriteLine("");
            Console.WriteLine($"- NAME: {user.Name} {user.Surname}\n- ADDRESS: {user.Address}\n- PHONE NO.: {user.PhoneNumber}\n \n- ITEM TO RETURN: {user.ItemDesc}");
            Console.WriteLine("");
            string infoCheck = Console.ReadLine();

            // Note: You may notice that even though we specifically ask for "Y" or "N" as an input...
            // I have put "Yes" and "No" as acceptable options too, to act as a bit of an invisible safety net.
            if (infoCheck == "Y" || infoCheck == "Yes")
            {
                return;
                // ^ Return back to our Main method to continue 
            }
            else if (infoCheck == "N" || infoCheck == "No")
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                getUserInfo();
                // ^ Wipe the screen and call the info-form function again
            }
        }

        // This validates the user's phone number (numeric and 7-10 digits long)
        static string GetValidPhoneNumber()
        {
            string phone;
            do
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine($"Welcome {user.Name}! Please begin by filling out some basic customer info below...");
                Console.WriteLine("---");
                Console.WriteLine(""); // < Padding
                Console.WriteLine("Please enter your PHONE NUMBER (no spaces or dashes):");
                Console.WriteLine(""); // < Padding
                phone = Console.ReadLine();
                if (!phone.All(char.IsDigit) || phone.Length < 7 || phone.Length > 11)
                {
                    Console.Clear();
                    Console.WriteLine("");
                    Console.WriteLine("ERROR");
                    Console.WriteLine("Invalid phone number. Please enter a valid phone number (no spaces or dashes):");
                }
            } while (!phone.All(char.IsDigit) || phone.Length < 7 || phone.Length > 11);

            return phone;
        }

        // Function to get our user's box dimensions
        static void GetBoxDimensions()
        {
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please enter the height of the box in cm (between 5 and 100 cm):");
            Console.WriteLine(""); // < Padding
            box.BoxHeight = GetValidDimension();

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please enter the width of the box in cm (between 5 and 100 cm):");
            Console.WriteLine(""); // < Padding
            box.BoxWidth = GetValidDimension();

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please enter the depth of the box in cm (between 5 and 100 cm):");
            Console.WriteLine(""); // < Padding
            box.BoxLength = GetValidDimension();

            Console.Clear();
            Console.WriteLine("------------------------------");
            //calculate the volume
            box.Volume = box.BoxHeight * box.BoxWidth * box.BoxLength;
            Console.WriteLine($"The volume of your box is: {box.Volume} cm³");
        }

        // Function to make sure the box dimensions are valid
        static double GetValidDimension()
        {
            double dimension;
            do
            {
                if (double.TryParse(Console.ReadLine(), out dimension) && dimension >= 5 && dimension <= 100)
                {
                    return dimension;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("");
                    Console.WriteLine("ERROR");
                    Console.WriteLine("Invalid input. Please enter a dimension between 5 and 100 cm:");
                }
            } while (true);
        }

        // Function to calculate the return cost for our user
        static void CalculateReturnCost(IslandsData[] islands)
        {
            Console.Clear();

            double baseRate = box.Volume <= 6000 ? 8.00 :
                            box.Volume <= 100000 ? 12.00 : 15.00;
            // ^ Using a ternary operator to calculate the base price for our user, before the island multipliers are applied.
            // (A ternary operator is a short-form way of an if-else statement; I am currently trying to learn about code
            // optimisation in C#, so I have chosen to use this slightly more advanced method for the sake of learning.)

            // Ask the user which NZ island they're returning from...
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Which island will you be returning the product from {user.Name}?\n(North Island, South Island, or Stewart Island)");
            Console.WriteLine(""); // < Padding
            string islandChoice = Console.ReadLine().ToUpper();

            // Find the island multiplier based on the selection
            IslandsData selectedIsland = islands.FirstOrDefault(i => i.islandName.ToUpper() == islandChoice);
            // ^  Searches for the first island in the array that matches the user's previous input (ignoring case).
            // Again, this is a little bit more a complex way of doing it for the sake of learning.
            // "FirstOrDefault" is a LINQ (Language Integrated Query) Method used for finding entries in arrays without
            // needing to use for-loops. It is looking for the "islandName" property of the array entry, and checking if
            // it matches the one from the user's input.

            if (selectedIsland != null)
            {
                user.SelectedIsland = selectedIsland.islandName;

                // Calculate the final return cost based on the selected island's multiplier
                double finalCost = baseRate * selectedIsland.rateMulti;

                // Print the return cost
                Console.WriteLine($"The return cost from {selectedIsland.islandName} is: ${finalCost:F2}");

                // Store the return cost in the user object
                user.returnCost = finalCost.ToString("F2"); // Formatting as a string with 2 decimal places
            }
            else
            {
                Console.WriteLine("Invalid island name entered. Please ensure you type it correctly (North Island, South Island, or Stewart Island).");
            }
        }

        static void PrintReceipt()
        {
            Console.WriteLine("----------");
            Console.WriteLine($"ORDER NUMBER: {user.OrderNumber}");
            Console.WriteLine($""); // Linebreak
            Console.WriteLine($"NAME: {user.Name} {user.Surname}");
            Console.WriteLine($"ITEM TO RETURN: {user.ItemDesc}");
            Console.WriteLine($"PHONE NUMBER: {user.PhoneNumber}");
            Console.WriteLine($"SENDER ADDRESS: {user.Address}");
            Console.WriteLine($"ISLAND OF RESIDENCE: {user.SelectedIsland}");
            Console.WriteLine($""); // Linebreak
            Console.WriteLine($"SUBTOTAL: {user.returnCost}");

            // I wanted to allow the program to cacluate GST for the user for convenience...
            double returnCost = Convert.ToDouble(user.returnCost); // < Convert the return cost subtotal to a double
            double totalWithGST = returnCost + (returnCost * 0.15); // < Add on the GST to calculate the Total

            Console.WriteLine($"TOTAL (+GST): {totalWithGST:F2}"); // < Print the total cost, using the same F2 formatting style as before
            Console.WriteLine("----------");
        }
    }
}