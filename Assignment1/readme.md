This project read data files (csv) from 'Sample Data' folder and process them one by one.
Files contains customer data of year 2017, 2018, 2019.
Program reads records based on following condition.
Condition for Invalid records as below. If either of below condition satisfies record will be an invalid record.
1. There will be 10 columns for each record which are : First Name,Last Name,Street Number,Street,City,Province,Postal Code,Country,Phone Number,email Address. If row contains less than 10 columns that will be invalid record.
2. Value for first name will be empty , null or non alphabetical characters.
3. value for street number  will be empty , null or non numeric.
4. value for street name will be empty , null or non alphanumeric.
5. value for zip code will be of empty , null or non alphanumeric or lenghth (after removing space) will not be 6 character long.
6. having non valid value of emaild address.(exaple : 1234, null@gmail.com, nk#@gmail.com, nittin#gmail.com).

Program will write all the valid record in result.csv file.
Further print the number of total record, invalid records, valid records into log file.

