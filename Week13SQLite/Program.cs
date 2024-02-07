
using System.Data.Common;
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

ReadData(CreateConnection());
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True; Compress = True;"); // ühendus db-ga
    try
    {
        connection.Open();
        Console.WriteLine("DB found.");
    }
    catch
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}
static void ReadData(SQLiteConnection myConnection) //kuvamine
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command; //päring
    command = myConnection.CreateCommand(); //ühendus tehtud lahti, päring loodud
    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader(); // hakkab lugema ja salvestama andmeid

    while (reader.Read()) //niikaua kui ta loeb,taidab:       read nagu boolean, niikaua kui "true"
    {
        string readerRowId = reader["rowid"].ToString();  // 2 voimalus - reader["rowid"].ToString ---> siis jarjekorra ei mojuta
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string  readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstName} {readerStringLastName}; DateOfBirth: {readerStringDoB}");
    }
    myConnection.Close();
}
static void InsertCustomer(SQLiteConnection myConnection) //andmete lisamine
{
    SQLiteCommand command;
    string fName, lName, dob;
    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter your date of birth (mm-dd-yyyy):");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = "INSERT INTO customer(firstName, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{dob}')";
    int rowInsert = command.ExecuteNonQuery();
    Console.WriteLine($"Rows inserted: {rowInsert}");

     ReadData(myConnection);
}

static void RemoveCustomer(SQLiteConnection myConnection)  //andme kustutamine
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter an id to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";

    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer");

    ReadData(myConnection);
}
 static void FindCustomer(SQLiteConnection myConnection)
{
    string searchParameter;
        Console.WriteLine("Enter the first name, last name, or id to find a customer:");
    searchParameter = Console.ReadLine();

    SQLiteCommand command = myConnection.CreateCommand();
    command.CommandText = $"SELECT rowid, * FROM customer " +
        $"WHERE firstName LIKE '{searchParameter}' OR lastName LIKE '{searchParameter}' OR rowid LIKE '{searchParameter}' ";

    SQLiteDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        Console.WriteLine("Matching customers found:");
        while (reader.Read())
        {
            string readerRowId1 = reader["rowid"].ToString();
            string readerStringFirstName1 = reader.GetString(1);
            string readerStringLastName1 = reader.GetString(2);
            string readerStringDoB1 = reader.GetString(3);

            Console.WriteLine($"{readerRowId1}. Full name: {readerStringFirstName1} {readerStringLastName1}; DateOfBirth: {readerStringDoB1}");
        }
    }
    else
    {
        Console.WriteLine("No matching customers found.");
    }

    reader.Close();
 }