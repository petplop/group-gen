using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GroupGen
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string selectedFile = null;
            var t = new Thread((ThreadStart)(() =>
            {
                while (selectedFile == null)
                {
                    var fileContent = string.Empty;
                    var filePath = string.Empty;

                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Get the path of specified file
                            selectedFile = openFileDialog.FileName;
                        }
                    }

                    int yes_or_no = 0;
                    Console.WriteLine("File not selected, would you like to try again?\n" +
                        "\nPress 1 for YES" +
                        "\nPress 2 for EXIT");

                    bool isValid = int.TryParse(Console.ReadLine(), out yes_or_no);

                    if (isValid)
                    {
                        if (yes_or_no == 1)
                        {
                            Console.WriteLine("\nTry again\n");
                        }
                        if (yes_or_no == 2)
                        {
                            Console.WriteLine("\nExiting...");
                            System.Threading.Thread.Sleep(1000);
                            System.Environment.Exit(1);
                        }
                    }
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            //Console.WriteLine(selectedFile);

            // Read .txt file and convert names to list
            string name_raw = File.ReadAllText(selectedFile);
            string[] name_list = name_raw.Split('\n');

            // Print names to make sure it's working
            foreach (string name in name_list)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine($"\nMaking groups with {name_list.Length} people...\n");

            //Taking user input and validating it
            Console.WriteLine("How many people in a group max?");
            int names_per_group = 0;
            bool isValid = int.TryParse(Console.ReadLine(), out names_per_group);

            int total_groups = name_list.Length / names_per_group;

            Random rand = new Random();
            List<string> names_shuffled = name_list.OrderBy(x => rand.Next()).ToList();

            //Turning "total_groups" var into list so I can print Group1, Group2... below via. index
            int[] total_groups_list = new int[total_groups];

            int GroupNum = 1;
            // Distributing groups if conversion of user input was successful
            if (isValid)
            {
                Console.WriteLine("Creating groups...");

                System.Threading.Thread.Sleep(1000);

                for (int i = 0; i < name_list.Length; i++)
                {
                    if (i % names_per_group == 0) // Starts new group when names_per_group has been maxed.
                    {
                        Console.WriteLine($"\nGroup: {GroupNum++} \n"); // Increases the group number for every iteration
                        System.Threading.Thread.Sleep(1000); // for cool effects
                    }
                    Console.WriteLine(names_shuffled[i]); // assigns name to group as long as names_per_max hasn't been met
                    System.Threading.Thread.Sleep(1000); // more cool effects
                }

                //below couldn't cope with if i % names_per_group != 0;

                //// Outputting group number with outer loop, and assigning the names_per_group under every group
                //for (int g = 0; g < total_groups_list.Length; g++)
                //{
                //    Console.WriteLine($"\nGroup {total_groups_list[g]} \n");

                //    System.Threading.Thread.Sleep(1000);

                //    for (int j = 0; j < names_per_group; j++)
                //    {
                //        //Formula to calculate correct array index y*N+x
                //        Console.WriteLine(name_list[g*names_per_group+j]);
                //        System.Threading.Thread.Sleep(1000);
                //    }
                //}
            }
            // If user input isn't a valind integer
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
