﻿using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

namespace Assignment1
{
    class Program
    {


        static UInt64 validRowCount = 0;
        static UInt64 totalRecordCount = 0;
        static UInt64 invalidRowCount = 0;
        Logger log;
        string rootfolder;
        String log_path ;
        StreamWriter w;
        public Program()
        {
            this.log = new Logger();
            this.rootfolder = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            this.log_path = rootfolder + "logs/log.txt";
            this. w = new StreamWriter(log_path, false);
        }




        static void Main(string[] args)
        {
            try
            {
                DateTime start = DateTime.Now;
                Program pr = new Program();
                Logger log = pr.log;
                StreamWriter w = pr.w;
                string rootfolder = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
                String fileDir = rootfolder + "Sample Data/";
                log.Log("root folder " + rootfolder,w);

                String outputDir = rootfolder + "output/";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                String resultFilepath = outputDir + "result.csv";
                if (File.Exists(resultFilepath))
                {
                    File.Delete(resultFilepath);
                }



                StreamWriter sw = pr.OpenStream(resultFilepath);
                log.Log("writing header into file ", w);
                sw.WriteLine("First Name,Last Name,Street Number,Street,City,Province,Postal Code,Country,Phone Number,email Address,File Date");

                pr.walk(fileDir, sw);
                sw.Close();
                DateTime end = DateTime.Now;

                TimeSpan ts = (end - start);
                log.Log("total number of total records is " + totalRecordCount, w);
                log.Log("total number of invalid records is " + invalidRowCount, w);
                log.Log("total number of valid records is " + validRowCount, w);
                log.Log("time taken by program is " + ts.TotalMilliseconds + " ms.", w);

                w.Close();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }



        public void walk(String path,StreamWriter sw)
        {
            try
            {
                string[] list = Directory.GetDirectories(path);



                if (list == null) return;

                foreach (string dirpath in list)
                {
                    if (Directory.Exists(dirpath))
                    {
                        walk(dirpath, sw);
                        Console.WriteLine("Dir:" + dirpath);
                    }
                }
                string[] fileList = Directory.GetFiles(path);
                foreach (string filepath in fileList)
                {

                    Console.WriteLine("File:" + filepath);
                    if (filepath.EndsWith(".csv"))
                    {
                        Console.WriteLine("reading file : " + filepath);
                        parse(filepath, sw);
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void parse(String fileName, StreamWriter sw)
        {
            
            try
            {
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int i = 0;
                    string [] filenameSplit = fileName.Split("/");
                    String filedate = filenameSplit[filenameSplit.Length - 4] + "/" + filenameSplit[filenameSplit.Length - 3] +"/"+filenameSplit[filenameSplit.Length - 2];
                    while (!parser.EndOfData)
                    {
                       string[] fields = parser.ReadFields();
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }
                        totalRecordCount++;
                        Boolean isValidRow = validateRow(fields, fileName);
                        if (!isValidRow) {
                            invalidRowCount++;
                            continue;
                        }
                        validRowCount++;
                        string result = string.Join(",", fields);
                        result = String.Concat(result, ",", filedate);
                       sw.WriteLine(result);
                    }
                }

            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }
        public  Boolean validateRow(String[] rowFields,String fileName) {
            Boolean isValid = true;
            if (rowFields.Length != 10)
            {
                log.Log("Record is invalid, count of column is not valid", w);
                isValid = false;
                return isValid;
            }
            String firstname = rowFields[0].Replace(" ","");
            String streetnum = rowFields[2];
            String streetname = rowFields[3].Replace(" ", ""); 
            String zipcode = rowFields[6].Replace(" ", ""); 
            String email = rowFields[9].Replace(" ", ""); 

            //Console.WriteLine(firstname + "  " + streetnum+"  " + streetname + " " + zipcode + " " + email);
            if (!isValidAlphaString(firstname) || String.IsNullOrEmpty(firstname)) {
               
                log.Log("Record is invalid, FirstName is null or empty or non alpha numeric", w);
                isValid = false;

            }
            else if (!isValidNumeric(streetnum) || String.IsNullOrEmpty(streetnum))
            {

                log.Log("Record is invalid, Street number value is null or empty or invalid number", w);
                isValid = false;

            }
            else if (!isValidAlphaNumeric(streetname) || String.IsNullOrEmpty(streetname))
            {

                log.Log("Record is invalid, Street name value is null or empty or non - alpha numeric ", w);
                isValid = false;

            }
            
            else if (!(zipcode.Trim().Length == 6) && !isValidAlphaNumeric(zipcode)|| String.IsNullOrEmpty(zipcode))
            {
                
                log.Log("Record is invalid, Postal Code is null or empty length shlould is 6 or non alpha numeric", w);
                isValid = false;

            }
           
            else if (!isValidEmail(email))
            {
                
                log.Log("Record is invalid, email is null or empty or not valid format emails : "+ rowFields[9], w);
                isValid = false;

            }
            if (!isValid) {

                log.Log("record is invalid for file " + fileName + "\nvalue of row is " + string.Join(",", rowFields), w);
            }

            return isValid;


        }
         StreamWriter OpenStream(string path)
        {
            if (path is null)
            {
                log.Log("You did not supply a file path.",w);
                return null;
            }

            try
            {
                var fs = new FileStream(path, FileMode.CreateNew);
                return new StreamWriter(fs);
            }
            catch (FileNotFoundException)
            {
                log.Log("The file or directory cannot be found.",w);
            }
            catch (DirectoryNotFoundException)
            {
                log.Log("The file or directory cannot be found.",w);
            }
            catch (DriveNotFoundException)
            {
                log.Log("The drive specified in 'path' is invalid.",w);
            }
            catch (PathTooLongException)
            {
                log.Log("'path' exceeds the maxium supported path length.",w);
            }
            catch (UnauthorizedAccessException)
            {
                log.Log("You do not have permission to create this file.",w);
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                log.Log("There is a sharing violation.",w);
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                log.Log("The file already exists.",w);
            }
            catch (IOException e)
            {
                log.Log($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}",w);
            }
            return null;
        }

        public bool isValidEmail(string emailId) {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            bool isValid = false;
           try{
               if (Regex.IsMatch(emailId, pattern))
            {
                isValid = true;
            }
            else {
                isValid = false;
            }
        }
            catch (ArgumentNullException)
            {

                log.Log("Exception: ArgumentNullException : Null argument has been passed", w);
            }
            catch (RegexMatchTimeoutException)
            {

                log.Log("Exception: ArgumentNullException : A time-out occurred while pattern matching", w);
            }
            return isValid;

        }
            public bool isValidAlphaNumeric(string value)
            {
                string pattern = "^[a-zA-Z0-9]*$";
                bool isValid = false;
                try { 
                if (Regex.IsMatch(value, pattern))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
                catch (ArgumentNullException)
                {

                    log.Log("Exception: ArgumentNullException : Null argument has been passed", w);
                }
                catch (RegexMatchTimeoutException)
                {

                    log.Log("Exception: ArgumentNullException : A time-out occurred while pattern matching", w);
                }
                return isValid;
            
            }
        public bool isValidNumeric(string value)
        {
            string pattern = @"^-?[0-9][0-9,\.]+$";
            bool isValid = false;
            try { 
            if (Regex.IsMatch(value, pattern))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
        }
            catch (ArgumentNullException)
            {

                log.Log("Exception: ArgumentNullException : Null argument has been passed", w);
            }
            catch (RegexMatchTimeoutException)
            {

                log.Log("Exception: ArgumentNullException : A time-out occurred while pattern matching", w);
            }
            return isValid;
            
        }
        public  bool isValidAlphaString(string value)
        {
            string pattern = "^[a-zA-Z]*$";
            bool isValid = false;
            try
            {
                if (Regex.IsMatch(value, pattern))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (ArgumentNullException) {

                log.Log("Exception: ArgumentNullException : Null argument has been passed",w);
            }
            catch (RegexMatchTimeoutException)
            {

                log.Log("Exception: ArgumentNullException : A time-out occurred while pattern matching", w);
            }
            return isValid;

        }
    }
}
