This project reads data files (csv) from 'Sample Data' folder and process them one by one.
Files contains customer data of year 2017, 2018, 2019.
Program reads records based and behaves as below.
Condition for Invalid records as below. If either of below condition satisfies record will be treated as an invalid record.
1. There are  10 columns for each record which are : First Name,Last Name,Street Number,Street,City,Province,Postal Code,Country,
   Phone Number,email Address. If row contains less than or more than, 10  columns then, that will be  marked as invalid record.
2. If value for first name will be empty , null or non alphabetical characters.
3. If value for street number  will be empty , null or non numeric.
4. If value for street name will be empty , null or non-alphanumeric.
5. If value for zip code will be of empty , null or non-alphanumeric or lenghth (after removing white space) will not be 6 character long.
6. If a record have a non valid value of emaild address.(example : 1234, null@gmail.com, nk#@gmail.com, nittin#gmail.com).

Program will write all the valid record in result.csv file.
Further print the number of total record, invalid records, valid records, total time of execution into log file.

