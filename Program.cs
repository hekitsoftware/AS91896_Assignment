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
            IslandsData[] islandOptions = new IslandsData[3];
            islandOptions[0] = new IslandsData("North Island", 1);
            islandOptions[1] = new IslandsData("South Island", 1.5);
            islandOptions[2] = new IslandsData("Stewart Island", 2);
            // ^ An array with 2 objects per entry, which is set up to be easily added upon
            //   for the potential other islands mentioned in the breif.

            Console.WriteLine("------------------------------");
            Console.WriteLine("Welcome to the ONLINZ Return-Cost Calculator");

            Console.WriteLine("Please Enter your FIRST NAME");
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
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Thank you {user.Name} for using ONLINZ for your package returning needs.");
            Console.WriteLine("Here is your recepit:");

            // Call the method to print the recepit
            PrintReceipt();

            Console.WriteLine("Would you like to calculate the return cost for another item? (Y/N)");
            string continueChoice = Console.ReadLine().ToUpper();
            if (continueChoice == "Y" || continueChoice == "YES")
            {
                // Restart the process if the user chooses to calculate again
                Main(args);
            }
            else
            {
                Console.WriteLine("Thank you for using the ONLINZ Return-Cost Calculator!");
                // Exit the program
            }

        }

        static void getUserInfo()
        {
            // Ask for the user's info...
            Console.WriteLine("Please enter your SURNAME:");
            user.Surname = Console.ReadLine().ToUpper();
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please enter your HOME/SHIPPING ADDRESS:");
            user.Address = Console.ReadLine().ToUpper();
            Console.Clear();
            Console.WriteLine("------------------------------");
            user.PhoneNumber = GetValidPhoneNumber();
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please breifly describe the ITEM you wish to return in 6 words or less:");
            user.ItemDesc = Console.ReadLine().ToUpper();

            //Print a copy of the entered data to check if the user has entered their data correctly...
            Console.WriteLine("------------------------------");
            Console.WriteLine("Thank you for entering your information... Does this look right? \n(Type 'Y' for Yes, or 'N' to re-enter your details)");
            Console.WriteLine($"- NAME: {user.Name} {user.Surname}\n- ADDRESS: {user.Address}\n- PHONE NO.: {user.PhoneNumber}");
            Console.WriteLine("");
            string infoCheck = Console.ReadLine();
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
                Console.WriteLine("Please enter your PHONE NUMBER:");
                phone = Console.ReadLine();
                if (!phone.All(char.IsDigit) || phone.Length < 7 || phone.Length > 11)
                {
                    Console.WriteLine("Invalid phone number. Please enter a valid phone number.");
                }
            } while (!phone.All(char.IsDigit) || phone.Length < 7 || phone.Length > 11);

            return phone;
        }

        // Function to get our user's box dimensions
        static void GetBoxDimensions()
        {
            Console.WriteLine("Please enter the height of the box in cm (between 5 and 100 cm):");
            box.BoxHeight = GetValidDimension();

            Console.WriteLine("Please enter the width of the box in cm (between 5 and 100 cm):");
            box.BoxWidth = GetValidDimension();

            Console.WriteLine("Please enter the depth of the box in cm (between 5 and 100 cm):");
            box.BoxLength = GetValidDimension();

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
                    Console.WriteLine("Invalid input. Please enter a dimension between 5 and 100 cm.");
                }
            } while (true);
        }

        // Function to calculate the return cost for our user
        static void CalculateReturnCost(IslandsData[] islands)
        {
            double baseRate = box.Volume <= 6000 ? 8.00 :
                            box.Volume <= 100000 ? 12.00 : 15.00;
            // ^ Using a ternary operator to calculate the base price for our user, before the island multipliers are applied.
            // (A ternary operator is a short-form way of an if-else statement; I am currently trying to learn about code
            // optimisation in C#, so I have chosen to use this slightly more advanced method for the sake of learning.)

            // Ask the user which NZ island they're returning from
            Console.WriteLine($"Which island will you be returning the product from {user.Name}?\n(North Island, South Island, or Stewart Island)");
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