namespace TeleprompterConsole;

using System.Data.SQLite;

internal class Program
{

    //update building general (non-floor specific) rows
    public static void UpdateRow_building(string connectionString, int ID_Building, string date, double[] array)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            for (int i = 0; i < array.Length; i++)
            {
                // Inserta una fila de datos
                using (SQLiteCommand command = new SQLiteCommand(
                    "UPDATE Building_Data SET Value = @Value WHERE ID_Building = @ID_Building AND ID_TypeData = @ID_TypeData AND Data = @Data AND Time = @Time", connection))
                {
                    command.Parameters.AddWithValue("@ID_Building", ID_Building);
                    command.Parameters.AddWithValue("@ID_TypeData", 3);
                    command.Parameters.AddWithValue("@Data", date);
                    command.Parameters.AddWithValue("@Time", i);
                    command.Parameters.AddWithValue("@Value", array[i]);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Update successful. {rowsAffected} row(s) affected.");
                    }
                    else
                    {
                        Console.WriteLine("No rows updated. Verify your ID and try again.");
                    }
                }
            }
            connection.Close();
        }
    }

    //update floor specific database rows
    public static void UpdateRow_floor(string connectionString, int ID_Floor, string date, double[] array)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            //for (int i = 0; i < array.Length; i++)
            for (int i = 0; i < array.Length; i++)
            {
                // Inserta una fila de datos
                using (SQLiteCommand command = new SQLiteCommand(
                    "UPDATE Data_Floor_Building SET Value = @Value WHERE ID_Building = @ID_Building AND ID_Floor = @ID_Floor AND ID_DataType = @ID_DataType AND Data = @Data AND Time = @Time", connection))
                {
                    command.Parameters.AddWithValue("@ID_Building", 2);
                    command.Parameters.AddWithValue("@ID_Floor", ID_Floor);
                    command.Parameters.AddWithValue("@ID_DataType", 3);
                    command.Parameters.AddWithValue("@Data", date);
                    command.Parameters.AddWithValue("@Time", i);
                    command.Parameters.AddWithValue("@Value", array[i]);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Update successful. {rowsAffected} row(s) affected.");
                    }
                    else
                    {
                        Console.WriteLine("No rows updated. Verify your ID and try again.");
                    }
                }
            }
            connection.Close();
        }
    }

    //enhanced APK2 water_gen algorithm; ONLY EXECUTE FOR BUILDING 2
    public static double[] Water_gen_APK2(int Floor, int numPoints)
    {
        double[] waterConsumption = new double[numPoints];

        double peakValue;
        double plateauValue;
        double afternoonPeak;
        double declineValue;
        double baselineValue;

        if (Floor == -1) { peakValue = 852; plateauValue = 379; afternoonPeak = 820; declineValue = 291; baselineValue = 101; }
        else { peakValue = 852 / 3; plateauValue = 379 / 3; afternoonPeak = 820 / 3; declineValue = 291 / 3; baselineValue = 101 / 3; }

        double minVariation = -5;
        double maxVariation = 5;

        Random random = new();

        for (int time = 0; time < numPoints; time++)
        {
            if (time >= numPoints / 3 && time < (2 * numPoints) / 5) // Morning peak (8:00 AM - 10:00 AM)
            {
                double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                waterConsumption[time] = peakValue + Math.Round(randomVariation, 1);
            }
            //this is the else if statement where the simulation will happen
            else if (time >= (2 * numPoints) / 5 && time < (3 * numPoints) / 4) // Midday plateau (10:00 AM - 4:00 PM)
            {

                if (time >= 17 * numPoints / 36 && time <= 73 * numPoints / 144) // 11:20 - 12:10 (50 minute period, water leakage begins)
                {
                    waterConsumption[time] = 1200; //arbitrary value, needs to be above 1000 for alarm to go off

                }
                else if (time > 73 * numPoints / 144 && time <= 13 * numPoints / 24) //12:10 - 13:00 (50 minute period, water has been cut)
                {
                    waterConsumption[time] = 0; //arbitrary value, needs to be above 1000
                }
                else //leakage has been fixed, back to normal behaviour
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    waterConsumption[time] = plateauValue + Math.Round(randomVariation, 1);
                }
            }
            else if (time >= (3 * numPoints) / 4 && time < (8 * numPoints) / 10) // Afternoon peak (4:00 PM - 5:00 PM)
            {
                double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                waterConsumption[time] = afternoonPeak + Math.Round(randomVariation, 1);
            }
            else if (time >= (8 * numPoints) / 10 && time < numPoints) // Evening decline (6:00 PM - 8:00 PM)
            {
                double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                waterConsumption[time] = declineValue + Math.Round(randomVariation, 1);
            }
            else // Nighttime baseline (8:00 PM - 8:00 AM)
            {
                double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                waterConsumption[time] = baselineValue + Math.Round(randomVariation, 1);
            }
        }

        return waterConsumption;
    }

    static void Main()
    {
        Console.WriteLine("APK2");
        int numberData = 1440;
        string currentDate = "30/11/23";
        string connectionString = "Data Source=/Users/shumbabala/Documents/VSCode/DataGenerationTest/DB General.db"; // <<-- posar path a la database

        //water_gen array intialization
        double[] water_gen;

        //loop to update building 2 values and all of its 3 floors (4 iterations)
        for (int i = 0; i < 1; i++)
        {
            water_gen = Water_gen_APK2(i - 1, numberData);

            if (i == 0)
            {
                UpdateRow_building(connectionString, 2, currentDate, water_gen);
            }
            else
            {
                UpdateRow_floor(connectionString, i, currentDate, water_gen);
            }
        }
    }
}