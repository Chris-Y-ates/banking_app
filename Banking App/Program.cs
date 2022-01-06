using System.IO;
using System;
using System.Linq;

namespace BankingApp
{
    public class Calculations
    {
        public string Answer; //user input
        public string path; //path to CSV file

        public void Greeting() //method for initial message displayed when program is run
        {
            Console.Write("\n" + "Welcome to the Banking App! " + "\n" + "\n" + "Please Select from the Following Options:" +
                "\n" + "\n" + "[1] See all Accounts" + "\n" + "[2] See Overdrawn Accounts" + "\n" + "[3] Add New Account" + "\n" +
                "[4] Make Withdrawal" + "\n" + "[5] Make Deposit" + "\n" + "\n" + "Enter Number Followed by Return: ");
        }
        public void Option1(string[] lines)//method for option 1 and array of lines of CSV file is parsed
        {
            Console.WriteLine(" ");

            for (int i = 0; i < lines.Length; i++) //loops through each line of array
            {
                string[] entries = lines[i].Split(','); //splits each line by comma to create array of individual elements

                for (int j = 0; j < entries.Length; j++) //loops through array of individual elements
                {
                    Console.Write(entries[j].PadRight(20)); //formats each element to be displayed in the terminal
                }
                Console.WriteLine(" "); //creates new line for formatting/aesthetic purposes
            }
        }
        public void Option2(string[] lines) //method for option 2 and array of lines of CSV file is parsed
        {
            Console.WriteLine(" "); //creates new line for formatting/aesthetic purposes

            for (int i = 0; i < lines.Length; i++)//loops through each line of array
            {
                string[] entries = lines[i].Split(',');//splits each line by comma to create array of individual elements
                bool endline = false; //boolean check to see if iteration is at end of line then insert newline for formatting
                
                for (int p = 0; p < entries.Length; p++)
                {
                    if (i == 0)
                {
                    Console.Write(entries[p].PadRight(20));

                        if (p == entries.Length - 1)
                        {
                            Console.WriteLine(" "); //creates new line for formatting/aesthetic purposes
                        }
                }
                    else if (float.Parse(entries[entries.Length - 2]) < 0.00)
                    {
                        Console.Write(entries[p].PadRight(20));
                        endline = true;
                    }

                    if (p == entries.Length - 1 && endline == true)
                    {
                        Console.WriteLine(" "); //creates new line for formatting/aesthetic purposes
                        endline = false;
                    }
                }
                //Console.WriteLine(" ");
            }
        }
        public void Option3(string path)
        {
            Console.Write("Enter Customer Name: "); //take new customers details
            string newName = Console.ReadLine();
            Console.Write("Enter NI Number: ");
            string newNI = Console.ReadLine();
            Console.Write("Enter Deposit to 2 Decimal Places: ");
            string actualDeposit = Console.ReadLine();
            double entryInterest = Math.Round(float.Parse(actualDeposit) * 0.025, 2); //calculates annual interest
            string appendText = Environment.NewLine + newName + "," + newNI + "," + actualDeposit + "," + entryInterest;
            File.AppendAllText(path, appendText); //updates CSV file with user input
            string readText = File.ReadAllText(path);
        }
        public void Option4or5(string[] lines, string path) //combined method for options 4 and 5
        {
            double savings_rate = 0.025; 
            double overdraft_rate = 0.10;
            double rate = savings_rate;

            Console.Write("Enter Customer NI Number: "); //check NI number against CSV file
            string ni_number = Console.ReadLine();

            Console.Write("Enter the Amount: "); //take amount to update CSV file
            string withdraw = Console.ReadLine();

            string appendText = ""; //initialise variable to insert new customer data into CSV file

            for (int i = 0; i < lines.Length; i++) //main loop to go through each line to insert updated information
            {
                string[] entries = lines[i].Split(','); //splits into iterable array

                if (entries[1] == ni_number) //once we have found a match for the NI number, need to insert new data
                {
                    if (Answer == "4")
                    {
                        float reduce = (float.Parse(entries[2]) - float.Parse(withdraw)); //acct balance minus withdrawl
                        if (reduce < 0.00)
                        {
                            rate = overdraft_rate; //if withdrawl puts account into overdraft, set interest rate to 10%
                        }
                        double reduced_interest = Math.Round(reduce * rate, 2); //sets int rate to 2 decimal places
                        string last_interest = reduced_interest.ToString(); //converts interest to string for CSV file
                        string reduced_balance = reduce.ToString(); //converts to string for CSV file
                        if (i == lines.Length - 1) //insert our figures at correct index
                        {
                            appendText += entries[0] + "," + entries[1] + "," + reduced_balance + "," + last_interest;
                        }
                        else //insert newline when at end of iteration
                        {
                            appendText += entries[0] + "," + entries[1] + "," + reduced_balance + "," + last_interest + 
                                Environment.NewLine;
                        }
                    }
                    else //if answer selected was option 5
                    {
                        float reduce = (float.Parse(entries[2]) + float.Parse(withdraw)); //balance + withdrawal calculation
                        double reduced_interest = Math.Round(reduce * rate, 2); //interest calculation to 2 decimal places
                        string last_interest = reduced_interest.ToString(); //converts to string for csv
                        string reduced_balance = reduce.ToString();
                        if (i == lines.Length - 1) //correct index so insert updated data
                        {
                            appendText += entries[0] + "," + entries[1] + "," + reduced_balance + "," + last_interest;
                        }
                        else //insert newline when at end of iteration
                        {
                            appendText += entries[0] + "," + entries[1] + "," + reduced_balance + "," + last_interest + 
                                Environment.NewLine;
                        }
                    }
                }
                else //all other data is re-inserted into CSV file unchanged
                {
                    if (i == lines.Length - 1)
                    {
                        appendText += entries[0] + "," + entries[1] + "," + entries[2] + "," + entries[3];
                    }
                    else
                    {
                        appendText += entries[0] + "," + entries[1] + "," + entries[2] + "," + entries[3] + Environment.NewLine;
                    }
                }
            }
            File.WriteAllText(path, appendText);//updates CSV file
        }
        public void Error() //if user does not enter any number from 1 to 5
        {
            Console.WriteLine("ERROR! Enter Single Number 1-5 Only");
        }

    }
     class Program
    {
        static void Main(string[] args)
        {
            Calculations dialogBox = new Calculations(); //creates new object
            string path = "data.csv"; //path to CSV file
            string[] lines = System.IO.File.ReadAllLines(path);
            dialogBox.Greeting(); //calls method for greeting message
            dialogBox.Answer = Console.ReadLine(); //captures option answer from user

            if (dialogBox.Answer == "1")
            {
                dialogBox.Option1(lines); //call method for option 1 and passes CSV data
            }

            else if (dialogBox.Answer == "2")
            {
                dialogBox.Option2(lines); //call method for option 2 and passes CSV data
            }

            else if (dialogBox.Answer == "3")
            {
                dialogBox.Option3(path); //call method for option 3 and passes path to CSV file
            }

            else if (dialogBox.Answer == "4" | dialogBox.Answer == "5")
            {
                dialogBox.Option4or5(lines, path); //call method for options 4 or 5 and passes CSV data
            }

            else
            {
                dialogBox.Error(); //call method for error message
            }
        }
    }
}
