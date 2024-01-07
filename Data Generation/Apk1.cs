namespace TeleprompterConsole;

using System.Data.SQLite;


internal class Program{
    public static void InsertRow_building(string connectionString, int ID_Building, int ID_DataType, string date , double[] array)
    {
    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        connection.Open();

        for(int i=0; i<array.Length; i++){
        // Inserta una fila de datos
        using (SQLiteCommand command = new SQLiteCommand(
            "UPDATE Building_Data SET Value = @Value WHERE ID_Building = @ID_Building AND ID_TypeData = @ID_TypeData AND Data = @Data AND Time = @Time", connection))            {
            command.Parameters.AddWithValue("@ID_Building", ID_Building);
            command.Parameters.AddWithValue("@ID_TypeData", ID_DataType);
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
    public static void UpdateRow_floor(string connectionString, int ID_Building, int ID_Floor, int ID_DataType, string date , double[] array)
    {
    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        connection.Open();

        for(int i=0; i<array.Length; i++){
        // Inserta una fila de datos
        using (SQLiteCommand command = new SQLiteCommand(
            "UPDATE Data_Floor_Building SET Value = @Value WHERE ID_Building = @ID_Building AND ID_Floor = @ID_Floor AND ID_DataType = @ID_DataType AND Data = @Data AND Time = @Time", connection))            
        {
            command.Parameters.AddWithValue("@ID_Building", ID_Building);
            command.Parameters.AddWithValue("@ID_Floor", ID_Floor);
            command.Parameters.AddWithValue("@ID_DataType", ID_DataType);
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
    public static void InsertRow_floor(string connectionString, int ID_Building, int ID_Floor, int ID_DataType, string date , double[] array)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            for(int i=0; i<array.Length; i++){
            // Inserta una fila de datos
            using (SQLiteCommand command = new SQLiteCommand(
                "INSERT INTO Data_Floor_Building (ID_Building, ID_Floor, ID_DataType, Data, Time, Value) VALUES (@ID_Building, @ID_Floor, @ID_DataType, @Data, @Time, @Value)", connection))
            {
                command.Parameters.AddWithValue("@ID_Building", ID_Building);
                command.Parameters.AddWithValue("@ID_Floor", ID_Floor);
                command.Parameters.AddWithValue("@ID_DataType", ID_DataType);
                command.Parameters.AddWithValue("@Data", date);
                command.Parameters.AddWithValue("@Time", i);
                command.Parameters.AddWithValue("@Value", array[i]);

                command.ExecuteNonQuery();
            }
            }
            connection.Close();
        }
    }
    

    public static double[] Temperature_gen_APK1(int Building, int Floor, int numValues)
        {
            Dictionary<(int, int), (double, double, int, int)> temperatureRanges = new Dictionary<(int, int), (double, double, int, int)>
            {
            // Building 1
            {(1, -1), (22, 27, 510, 560)},
            {(1, 1), (20, 25, 510, 560)},
            {(1, 2), (20.5, 25.5, 515, 565)},
            {(1, 3), (21, 26, 520, 570)},
            {(1, 4), (19, 24, 525, 575)},
            {(1, 5), (22, 26, 530, 580)}
            };
            (double minValue, double maxValue, int startFire, int midFire) = temperatureRanges.TryGetValue((Building, Floor), out var range) ? range : (0, 0,0,0);

            int Firefighters = 610;
            int endFire = 660; 

            int totalValues = numValues;
            double[] temperatureValues = new double[totalValues];

            double amplitude = (maxValue - minValue) / 2;
            double amplitude2 = 28.5;
            double offset = (maxValue + minValue) / 2;
            double angularFrequency = Math.PI / (totalValues / 2);
            double dernVal = 0;
            double linearVariation=0;
            int cpt = 0;


            for (int i = 0; i < totalValues; i++)
            {
                if(i>=startFire && i<midFire){
                    if(cpt==0){
                        linearVariation=(50 - temperatureValues[i-1]) / 50.0; 
                    }
                    cpt = 15;
                    dernVal = temperatureValues[i-1];
                    temperatureValues[i] = Math.Round(dernVal + linearVariation,2);

                }
                else if(i>=midFire && i<=Firefighters){
                    temperatureValues[i] = temperatureValues[i-1] ;

                }
                else if(i>Firefighters && i<endFire){
                    //linearVariation=(50 - temperatureValues[i-1]) / 50.0; 
                    dernVal = temperatureValues[i-1];
                    temperatureValues[i] = Math.Round(dernVal - linearVariation,2);
                }
                else{
                    double sinusoidalVariation = amplitude * Math.Sin(angularFrequency * (i - totalValues / 4));
                    double temperature = offset + sinusoidalVariation; //+ randomVariation;
                    temperatureValues[i] = Math.Round(temperature, 1);
                }
            }
            return temperatureValues;
        }

        public static double[] AQI_gen(int Building, int Floor, int numPoints)
        {
            double[] aqiValues = new double[numPoints];

            // Generar valores de calidad del aire (AQI) con variación diurna y nocturna
            Random random = new Random();

            Dictionary<(int, int), (double, double, int, int)> AqiRanges = new Dictionary<(int, int), (double, double, int, int)>
            {
            // Building 1
            {(1, -1), (30, 45, 510, 560)},
            {(1, 1), (30, 45, 510, 560)},
            {(1, 2), (30, 45, 515, 565)},
            {(1, 3), (30, 45, 520, 570)},
            {(1, 4), (30, 45, 525, 575)},
            {(1, 5), (30, 45, 530, 580)}
            };
            (double daytimeAqi, double nighttimeAqi, int startFire, int midFire) = AqiRanges.TryGetValue((Building, Floor), out var range) ? range : (0, 0,0,0);

            int Firefighters = 610;
            int endFire = 660; 
            // Definir el punto en el tiempo donde ocurre el cambio entre día y noche
            int nightTimeStart = numPoints / 3; // Cambio a la noche a las 8:00 PM
            double linearVariation=0; 
            double amplitude2 = 30;
            double offset = 30;
            int cpt=0;
            double dernVal = 0;


            for (int time = 0; time < numPoints; time++)
            {
                double aqi;
                if(time>=startFire && time<midFire){
                    if(cpt==0){
                        linearVariation= Math.Abs((0 - aqiValues[time-1]) / 50.0); 
                    }
                    cpt = 15;
                    dernVal = aqiValues[time-1];
                    aqiValues[time] = Math.Round(dernVal - linearVariation,2);

                }
                else if(time>=midFire && time<=Firefighters){
                    aqiValues[time] = aqiValues[time-1] ;

                }
                else if(time>Firefighters && time<endFire){
                    //linearVariation=(50 - temperatureValues[i-1]) / 50.0; 
                    dernVal = aqiValues[time-1];
                    aqiValues[time] = Math.Round(dernVal + linearVariation,2);
                }
                else 
                {
                    // Es de día: AQI es más bajo durante el día
                    aqi = random.Next((int)(0.95 * daytimeAqi), (int)(1.05 * daytimeAqi));
                    aqiValues[time] = aqi;
                }
            }
            return aqiValues;
        }

        public static double[] COCO2Level_gen(int Building, int Floor, int numPoints)
        {
            Dictionary<(int, int), (int, int)> CO2Ranges = new Dictionary<(int, int), (int, int)>
            {
            // Building 1
            {(1, -1), (510, 560)},
            {(1, 1), (510, 560)},
            {(1, 2), (515, 565)},
            {(1, 3), (520, 570)},
            {(1, 4), (525, 575)},
            {(1, 5), (530, 580)}
            };
            (int startFire, int midFire) = CO2Ranges.TryGetValue((Building, Floor), out var range) ? range : (0,0);
            int Firefighters = 610;
            int endFire = 660; 

            double[] coCo2Values = new double[numPoints];
            double linearVariation=0; 
            double amplitude2 = 40;
            double offset = 20;
            int cpt=0;
            double dernVal = 0;


            // Generar valores de CO/CO2 aleatorios
            Random random = new Random();

            for (int time = 0; time < numPoints; time++)
            {  
                if(time>=startFire && time<midFire){
                    if(cpt==0){
                        linearVariation= Math.Abs((60 - coCo2Values[time-1]) / 50.0); 
                    }
                    cpt = 15;
                    dernVal = coCo2Values[time-1];
                    coCo2Values[time] = Math.Round(dernVal + linearVariation,2);

                }
                else if(time>=midFire && time<=Firefighters){
                    coCo2Values[time] = coCo2Values[time-1] ;

                }
                else if(time>Firefighters && time<endFire){
                    //linearVariation=(50 - temperatureValues[i-1]) / 50.0; 
                    dernVal = coCo2Values[time-1];
                    coCo2Values[time] = Math.Round(dernVal - linearVariation,2);
                }
                else{
                    double coCo2 = random.Next(19, 23); // Valores entre 18 y 23 (pueden ser ppm)
                    coCo2Values[time] = coCo2;
                }
                

            }

            return coCo2Values;
        }
        
        public static double[] People_gen(int numPoints)
        {
 

            double[] peopleValues = new double[numPoints];
            double linearVariation=0; 
            double amplitude2 = 100;
            double amplitude3 = 60;
            double offset = 100;
            double offset2 = 120;

            Random random = new Random();

            for (int time = 0; time < numPoints; time++)
            {
                double basePeople=200;

                if(time>=521 && time<529){
                    peopleValues[time] = peopleValues[time-1] - 25;

                }
                else if(time>=529){
                    peopleValues[time] = peopleValues[time-1];

                }
                else if(time ==  520){
                    peopleValues[time] = 200;
                }
                else{
                    double randomVariation = random.Next(-2, 3);
                    peopleValues[time] = basePeople + randomVariation;
                }
            }

            return peopleValues;
        }
         public static double[] People_gen_update(int Building, int Floor, int numPoints)
        {
            double[] peopleValues = new double[numPoints];
            Random random = new Random();

            Dictionary<(int, int), (int,int)> PeopleRanges = new Dictionary<(int, int), (int,int)>
            {
            // Building 1
            {(1, 1), (500, 531)},
            {(1, 2), (400, 529)},
            {(1, 3), (300, 527)},
            {(1, 4), (300, 527)},
            {(1, 5), (300, 527)}
            };
            (int numberpeople,int timeppl) = PeopleRanges.TryGetValue((Building, Floor), out var range) ? range : (0,0);
            int Firefighters = 610;
            int endFire = 660; 

            for (int time = 0; time < numPoints; time++)
            {
                if(time>=521 && time<timeppl){
                    peopleValues[time] = peopleValues[time-1] - 50;

                }
                else if(time>=527){
                    peopleValues[time] = peopleValues[time-1];

                }
                else if(time ==  520){
                    peopleValues[time] = numberpeople;
                }
                else if(time>= 0 && time<= 479){
                    double randomVariation = random.Next(-2, 3);
                    peopleValues[time] = numberpeople/10 + randomVariation;
                }
                else{
                    double randomVariation = random.Next(-2, 3);
                    peopleValues[time] = numberpeople + randomVariation;
                }
            }

            return peopleValues;
        }



    static void Main()
    {
        Console.WriteLine("APK1");
        int numberData = 1440;
        string currentDate = "30/11/23";
        //string connectionString = "Data Source=C:\\Users\\delio\\source\\repos\\DB_final_filled1.db";
        string connectionString = "Data Source=C:\\Users\\delio\\source\\repos\\DB_apk1_updated.db";
        double[] tempval;
        double[] aqival;
        double[] co2val;
        double[] peopleVal;
        //tempval = Temperature_gen_APK1(1, -1, numberData);
        //aqival = AQI_gen(1, numberData);
        //co2val = COCO2Level_gen(1,numberData);
        //peopleVal = People_gen(numberData);
    
        //InsertRow_building(connectionString, 1, 1, currentDate, tempval);
        //InsertRow_building(connectionString, 1, 8, currentDate, aqival);
        //InsertRow_building(connectionString, 1, 12, currentDate, co2val);
        //InsertRow_building(connectionString, 1, 5, currentDate, peopleVal);

        // tempval = Temperature_gen_APK1(1, 1, numberData);
        // InsertRow_floor(connectionString, 1, 1, 1, currentDate, tempval);
        // for(int i=1; i<6; i++){
        //     tempval = Temperature_gen_APK1(1, i, numberData);
        //     InsertRow_floor(connectionString, 1, i, 1, currentDate, tempval);
        // }
        // for(int i=1; i<6; i++){
        //     aqival = AQI_gen(1, i, numberData);
        //     InsertRow_floor(connectionString, 1, i, 8, currentDate, aqival);
        // }
        // for(int i=1; i<6; i++){
        //     co2val = COCO2Level_gen(1, i, numberData);
        //     UpdateRow_floor(connectionString, 1, i, 12, currentDate, co2val);
        // }
        for(int i=1; i<6; i++){
            //tempval = Temperature_gen_APK1(1, i, numberData);
            peopleVal = People_gen_update(1, i,numberData);
            //co2val = COCO2Level_gen(1, i, numberData);
            //aqival = AQI_gen(1, i, numberData);

            // UpdateRow_floor(connectionString, 1, i, 1, currentDate, tempval);
            // UpdateRow_floor(connectionString, 1, i, 8, currentDate, aqival);
            // UpdateRow_floor(connectionString, 1, i, 12, currentDate, co2val);
            UpdateRow_floor(connectionString, 1, i, 5, currentDate,peopleVal);
            
            
        }


    }
    
}