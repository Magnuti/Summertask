using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace todoApp
{
    public class Program
    {
        // Wanted to use a dictionary, but cannon XmlSerialize dictionaries
        public List<List<string>> list = new List<List<string>>();
        public int counter;

        public void save(Program program)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Program));
            // Overrides the current file
            var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "values.txt", FileMode.Create);
            TextWriter writer = new StreamWriter(fileStream);
            ser.Serialize(writer, program);
            fileStream.Close();
        }

        public void add(string name)
        {
            this.counter++;
            List<string> l = new List<string>();
            l.Add(this.counter.ToString());
            l.Add(name);
            this.list.Add(l);
            Console.WriteLine("#" + counter + " " + name);
        }

        public void doTask(int number)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (Int32.Parse(this.list[i][0]) == number)
                {
                    Console.WriteLine("Completed #" + number + " " + this.list[i][1]);
                    this.list.RemoveAt(i);
                    return;
                }
            }
            Console.WriteLine("No entry with number " + number);
        }

        public void print()
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                Console.WriteLine("#" + this.list[i][0] + " " + this.list[i][1]);
            }
        }

        static void Main(string[] args)
        {
            Program program;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "values.txt";
            if (File.Exists(filePath))
            {
                System.Xml.Serialization.XmlSerializer xmlReader = new System.Xml.Serialization.XmlSerializer(typeof(Program));
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                program = (Program)xmlReader.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                program = new Program();
                System.IO.File.Create(filePath).Close();
                program.save(program);
            }
            Console.WriteLine("Todo App");
            while (true)
            {
                Console.WriteLine("Waiting for input...");
                string input = Console.ReadLine();
                string[] words = input.Split(" ");
                try
                {
                    if (words[0].Equals("Add"))
                    {
                        try
                        {
                            string s = "";
                            for (int i = 1; i < words.Length; i++)
                            {
                                s += words[i] + " ";
                            }
                            program.add(s);
                        }
                        catch
                        {
                            Console.WriteLine("Please give a name of the todo-element.");
                        }
                    }
                    else if (words[0].Equals("Do"))
                    {
                        try
                        {
                            program.doTask(Int32.Parse(words[1]));
                        }
                        catch
                        {
                            Console.WriteLine("Please specify a number after 'Do'.");
                        }
                    }
                    else if (words[0].Equals("Print"))
                    {
                        program.print();
                    }
                    else
                    {
                        Console.WriteLine(words[0] + " is not recognised as an internal command.");
                    }
                }
                catch
                {
                    Console.WriteLine("Unknown command.");
                }
                program.save(program);
            }
        }
    }
}
