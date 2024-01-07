namespace TeleprompterConsole;

using System.Data.SQLite;

internal class Program
{
    public class Data
    {
        public int Building { get; set; }
        public int Floor { get; set; }
        public int[] datatype = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        public string date { get; set; }

        public Data(int ID_Building, string data)
        {
            this.Building = ID_Building;
            Floor = -1;
            date = data; 
        }
        public Data(int ID_Building, int ID_Floor, string data)
        {
            this.Building = ID_Building;
            Floor = ID_Floor;
            date = data;
        }
    }
    public class Functions
    {
        public static void DisplayArrayWithUnits(double[] arr, string variableType, string units)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; // Establecer el color del texto a blanco (puede parecer negrita)
            Console.WriteLine("Variable type: " + variableType);
            Console.WriteLine("Units: " + units);
            Console.ForegroundColor = ConsoleColor.Black; // Restablecer el color del texto a gris (o el color predeterminado)
            Console.WriteLine("[{0}]", string.Join(", ", arr));
            Console.WriteLine(); // Agregar un salto de línea al final
        }


        public static void InsertRow_building(string connectionString, int ID_Building, int ID_DataType, string date , double[] array)
        {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            for(int i=0; i<array.Length; i++){
            // Inserta una fila de datos
            using (SQLiteCommand command = new SQLiteCommand(
                "INSERT INTO Building_Data (ID_Building, ID_TypeData, Data, Time, Value) VALUES (@ID_Building, @ID_TypeData, @Data, @Time, @Value)", connection))
            {
                command.Parameters.AddWithValue("@ID_Building", ID_Building);
                command.Parameters.AddWithValue("@ID_TypeData", ID_DataType);
                command.Parameters.AddWithValue("@Data", date);
                command.Parameters.AddWithValue("@Time", i);
                command.Parameters.AddWithValue("@Value", array[i]);

                command.ExecuteNonQuery();
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
    }

    public class Generation
    {

        public static double[] Temperature_gen(int Building, int Floor, int numValues)
        {
            int totalValues = numValues;
            double minValue;
            double maxValue;

            //Building 1
            if (Building == 1 && Floor == -1){minValue = 22; maxValue = 27;}
            else if (Building == 1 && Floor == 1){minValue = 20;maxValue = 25;}
            else if (Building == 1 && Floor == 2){minValue = 20.5;maxValue = 25.5;}
            else if (Building == 1 && Floor == 3){minValue = 21;maxValue = 26;}
            else if (Building == 1 && Floor == 4){minValue = 19;maxValue = 24;}
            else if (Building == 1 && Floor == 5){minValue = 22;maxValue = 26;}
            //Building 2
            else if (Building == 2 && Floor == -1){minValue = 21;maxValue = 26;}
            else if (Building == 2 && Floor == 1){minValue = 21;maxValue = 26;}
            else if (Building == 2 && Floor == 2){minValue = 19;maxValue = 24;}
            else if (Building == 2 && Floor == 3){minValue = 22;maxValue = 26;}
            //Building 3
            else if (Building == 3 && Floor == -1){minValue = 21.5;maxValue = 25;}
            else if (Building == 3 && Floor == 1){minValue = 20.5;maxValue = 25.5;}
            else if (Building == 3 && Floor == 2){minValue = 21;maxValue = 26;}
            else if (Building == 3 && Floor == 3){minValue = 19;maxValue = 24;}
            else if (Building == 3 && Floor == 4){minValue = 22;maxValue = 26;}
            else if (Building == 3 && Floor == 5){minValue = 20.5;maxValue = 25.5;}
            else if (Building == 3 && Floor == 6){minValue = 21;maxValue = 26;}
            else if (Building == 3 && Floor == 7){minValue = 19;maxValue = 24;}
            else
            {
                // Handle other cases or provide default values
                minValue = 0;
                maxValue = 0;
            }

            double amplitude = (maxValue - minValue) / 2;
            double offset = (maxValue + minValue) / 2;
            double angularFrequency = Math.PI / (totalValues / 2); // One full period in the middle

            double[] temperatureValues = new double[totalValues];

            for (int i = 0; i < totalValues; i++)
            {
                double sinusoidalVariation = amplitude * Math.Sin(angularFrequency * (i - totalValues / 4));
                double temperature = offset + sinusoidalVariation; //+ randomVariation;
                temperatureValues[i] = Math.Round(temperature, 1);
            }
            return temperatureValues;

        }

        public static double[] Electricity_gen(int Building, int Floor, int numPoints)
        {
            double peakValue;  // Example: 80 kWh
            double plateauValue;  // Example: 60 kWh
            double declineValue;  // Example: 50 kWh
            double baselineValue;  // Example: 30 kWh
            //Building 1
            if (Building == 1 && Floor == -1){peakValue = 100.0;  plateauValue = 70.0;  declineValue = 50.0; baselineValue = 20.0;}
            else if (Building == 1 && Floor == 1){peakValue = 20.0;  plateauValue = 15.0;  declineValue = 10.0; baselineValue = 3.0;}
            else if (Building == 1 && Floor == 2){peakValue = 25.0;  plateauValue = 20.0;  declineValue = 12.0; baselineValue = 7.0;}
            else if (Building == 1 && Floor == 3){peakValue = 15.0;  plateauValue = 10.0;  declineValue = 8.0; baselineValue = 2.0;}
            else if (Building == 1 && Floor == 4){peakValue = 23.0;  plateauValue = 15.0;  declineValue = 13.0; baselineValue = 5.0;}
            else if (Building == 1 && Floor == 5){peakValue = 17.0;  plateauValue = 10.0;  declineValue = 7.0; baselineValue = 3.0;}
            //Building 2
            else if (Building == 2 && Floor == -1){peakValue = 60.0;  plateauValue = 40.0;  declineValue = 30.0; baselineValue = 10.0;}
            else if (Building == 2 && Floor == 1){peakValue = 20.0;  plateauValue = 12.0;  declineValue = 10.0; baselineValue = 3.0;}
            else if (Building == 2 && Floor == 2){peakValue = 22.0;  plateauValue = 18.0;  declineValue = 15.0; baselineValue = 5.0;}
            else if (Building == 2 && Floor == 3){peakValue = 18.0;  plateauValue = 10.0;  declineValue = 5.0; baselineValue = 2.0;}
            //Building 3
            else if (Building == 3 && Floor == -1){peakValue = 140.0;  plateauValue = 100.0;  declineValue = 80.0; baselineValue = 30.0;}
            else if (Building == 3 && Floor == 1){peakValue = 20.0;  plateauValue = 12.0;  declineValue = 10.0; baselineValue = 3.0;}
            else if (Building == 3 && Floor == 2){peakValue = 22.0;  plateauValue = 18.0;  declineValue = 15.0; baselineValue = 5.0;}
            else if (Building == 3 && Floor == 3){peakValue = 18.0;  plateauValue = 10.0;  declineValue = 5.0; baselineValue = 2.0;}
            else if (Building == 3 && Floor == 4){peakValue = 20.0;  plateauValue = 12.0;  declineValue = 10.0; baselineValue = 3.0;}
            else if (Building == 3 && Floor == 5){peakValue = 25.0;  plateauValue = 18.0;  declineValue = 13.0; baselineValue = 7.0;}
            else if (Building == 3 && Floor == 6){peakValue = 30.0;  plateauValue = 20.0;  declineValue = 22.0; baselineValue = 8.0;}
            else if (Building == 3 && Floor == 7){peakValue = 15.0;  plateauValue = 10.0;  declineValue = 5.0; baselineValue = 2.0;}
            else
            {
                // Handle other cases or provide default values
                peakValue=0;  
                plateauValue=0;  
                declineValue=0;  
                baselineValue=0;  
            }
            double[] electricityConsumption = new double[numPoints];

            double minVariation = -0.5;
            double maxVariation = 0.5;

            Random random = new Random();


            for (int time = 0; time < numPoints; time++)
            {
                if (time >= numPoints / 3 && time < (2 * numPoints) / 5) // Morning peak (8:00 AM - 10:00 AM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    electricityConsumption[time] = peakValue + Math.Round(randomVariation, 1);
                }
                else if (time >= (2 * numPoints) / 5 && time < (3 * numPoints) / 4) // Midday plateau (10:00 AM - 4:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    electricityConsumption[time] = plateauValue + Math.Round(randomVariation, 1);
                }
                else if (time >= (3 * numPoints) / 4 && time < (8 * numPoints) / 10) // Afternoon peak (4:00 PM - 5:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    electricityConsumption[time] = peakValue + Math.Round(randomVariation, 1);
                }
                else if (time >= (8 * numPoints) / 10 && time < numPoints) // Evening decline (5:00 PM - 8:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    electricityConsumption[time] = declineValue + Math.Round(randomVariation, 1);
                }
                else // Nighttime baseline (8:00 PM - 8:00 AM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    electricityConsumption[time] = baselineValue + Math.Round(randomVariation, 1);
                }
                //electricityConsumption[time] = CalculateHourlyConsumption(time, numPoints);
            }

            return electricityConsumption;
        }


        public static double[] Water_gen(int Building, int Floor, int numPoints)
        {
            double[] waterConsumption = new double[numPoints];

            double peakValue;
            double plateauValue;
            double afternoonPeak;
            double declineValue;
            double baselineValue;

            //Building 1
            if (Building == 1 && Floor == -1) {peakValue = 1514;plateauValue = 757;afternoonPeak = 1324;declineValue = 567;baselineValue = 189;} 
            else if (Building == 1 && Floor >= 1 && Floor <= 5) {peakValue = 1514/5; plateauValue = 757/5; afternoonPeak = 1324/5; declineValue = 567/5;baselineValue = 189/5;}
            //Building 2
            else if (Building == 2 && Floor == -1) {peakValue = 852;plateauValue = 379;afternoonPeak = 820;declineValue = 291;baselineValue = 101;} 
            else if (Building == 2 && Floor >= 1 && Floor <= 3) {peakValue = 852/3; plateauValue = 379/3; afternoonPeak = 820/3; declineValue = 291/3;baselineValue = 101/3;}            
            // Building 3
            else if (Building == 3 && Floor == -1) {peakValue = 2341;plateauValue = 1115;afternoonPeak = 1947;declineValue = 863;baselineValue = 398;} 
            else if (Building == 3 && Floor >= 1 && Floor <= 7) {peakValue = 2341/7; plateauValue = 1115/7; afternoonPeak = 1947/7; declineValue = 863/7;baselineValue = 398/7;}
            else
            {
                // Handle other cases or provide default values
                peakValue=0;  
                plateauValue=0;  
                afternoonPeak = 0;
                declineValue=0;  
                baselineValue=0;  
            }

            double minVariation = -5;
            double maxVariation = 5;

            Random random = new Random();


            for (int time = 0; time < numPoints; time++)
            {
                if (time >= numPoints / 3 && time < (2 * numPoints) / 5) // Morning peak (8:00 AM - 10:00 AM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    waterConsumption[time] = peakValue + Math.Round(randomVariation, 1);
                }
                else if (time >= (2 * numPoints) / 5 && time < (3 * numPoints) / 4) // Midday plateau (10:00 AM - 4:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    waterConsumption[time] = plateauValue + Math.Round(randomVariation, 1);
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

        public static double[] Gas_gen(int Building, int Floor, int numPoints, int decimalPlaces = 3)
        {
            double[] gasConsumption = new double[numPoints];

            double peakValue = 2.5; // Consumo pico de gas (por ejemplo, 2.5 m³/h)
            double plateauValue = 1.0; // Consumo en meseta (por ejemplo, 1.0 m³/h)
            double afternoonPeak = 2.0; // Consumo pico de la tarde (por ejemplo, 2.0 m³/h)
            double declineValue = 0.5; // Disminución del consumo (por ejemplo, 0.5 m³/h)
            double baselineValue = 0.2; // Consumo base (por ejemplo, 0.2 m³/h)

            // Resto del código para definir los valores peakValue, plateauValue, afternoonPeak, declineValue, y baselineValue

            double minVariation = -0.001;
            double maxVariation = 0.001;

            Random random = new Random();

            for (int time = 0; time < numPoints; time++)
            {
                if (time >= numPoints / 3 && time < (2 * numPoints) / 5) // Morning peak (8:00 AM - 10:00 AM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    gasConsumption[time] = Math.Round(peakValue + randomVariation, decimalPlaces);
                }
                else if (time >= (2 * numPoints) / 5 && time < (3 * numPoints) / 4) // Midday plateau (10:00 AM - 4:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    gasConsumption[time] = Math.Round(plateauValue + randomVariation, decimalPlaces);
                }
                else if (time >= (3 * numPoints) / 4 && time < (8 * numPoints) / 10) // Afternoon peak (4:00 PM - 5:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    gasConsumption[time] = Math.Round(afternoonPeak + randomVariation, decimalPlaces);
                }
                else if (time >= (8 * numPoints) / 10 && time < numPoints) // Evening decline (6:00 PM - 8:00 PM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    gasConsumption[time] = Math.Round(declineValue + randomVariation, decimalPlaces);
                }
                else // Nighttime baseline (8:00 PM - 8:00 AM)
                {
                    double randomVariation = random.NextDouble() * (maxVariation - minVariation) + minVariation;
                    gasConsumption[time] = Math.Round(baselineValue + randomVariation, decimalPlaces);
                }
            }

            return gasConsumption;
        }

        public static double[] SolarRadiation_gen(int Building, int numPoints)
        {
            double[] solarRadiation = new double[numPoints];

            int totalValues = numPoints;
            double minValue;
            double maxValue;

            if(Building==1){minValue = 0;maxValue = 1000;}
            else if(Building == 2){ minValue = 0; maxValue = 750;}
            else if(Building == 3){ minValue = 0; maxValue = 1300;}
            else{minValue = 0;maxValue = 0;}

            double amplitude = (maxValue - minValue) / 2;
            double offset = (maxValue + minValue) / 2;
            double angularFrequency = Math.PI / (totalValues / 2); // One full period in the middle

            for (int i = 0; i < totalValues; i++)
            {
                double sinusoidalVariation = amplitude * Math.Sin(angularFrequency * (i - totalValues / 4));
                double temperature = offset + sinusoidalVariation; //+ randomVariation;
                solarRadiation[i] = Math.Round(temperature, 1);
            }
            return solarRadiation;
        }

        public static double[] SolarPanels_gen(int numPoints, double[] solarRad)
        {
            double[] solarPanels = new double[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                solarPanels[i] = Math.Round(0.2 * solarRad[i], 1); // Calculate the enrgy in kwH
            }

            return solarPanels;
        }


        public static double[] AQI_gen(int Building, int numPoints)
        {
            double[] aqiValues = new double[numPoints];

            // Generar valores de calidad del aire (AQI) con variación diurna y nocturna
            Random random = new Random();

            // Definir valores promedio para el día y la noche
            double daytimeAqi;
            double nighttimeAqi;

            // Establecer valores según el edificio
            if (Building == 1)
            {
                daytimeAqi = 50; // Valor de AQI durante el día en Building 1
                nighttimeAqi = 65; // Valor de AQI durante la noche en Building 1
            }
            else if (Building == 2)
            {
                daytimeAqi = 45; // Valor de AQI durante el día en Building 2
                nighttimeAqi = 60; // Valor de AQI durante la noche en Building 2
            }
            else if (Building == 3)
            {
                daytimeAqi = 55; // Valor de AQI durante el día en Building 3
                nighttimeAqi = 70; // Valor de AQI durante la noche en Building 3
            }
            else
            {
                daytimeAqi = 50; // Valor de AQI predeterminado para otros casos
                nighttimeAqi = 65; // Valor de AQI predeterminado para otros casos
            }

            // Definir el punto en el tiempo donde ocurre el cambio entre día y noche
            int nightTimeStart = numPoints / 3; // Cambio a la noche a las 8:00 PM

            for (int time = 0; time < numPoints; time++)
            {
                double aqi;

                if (time < nightTimeStart)
                {
                    // Es de día: AQI es más bajo durante el día
                    aqi = random.Next((int)(0.8 * daytimeAqi), (int)(1.2 * daytimeAqi));
                }
                else
                {
                    // Es de noche: AQI es peor por la inversión de temperatura
                    aqi = random.Next((int)(0.8 * nighttimeAqi), (int)(1.2 * nighttimeAqi));
                }

                aqiValues[time] = aqi;
            }

            return aqiValues;
        }



        public static double[] Humidity_gen(int Building, int numPoints)
        {
            double[] humidityValues = new double[numPoints];

            double morningHumidity = 95;   // Porcentaje de humedad en la mañana
            double middayHumidity = 35;    // Porcentaje de humedad al mediodía
            double afternoonHumidity = 50; // Porcentaje de humedad por la tarde
            double eveningHumidity = 40;   // Porcentaje de humedad por la noche
            double nightHumidity = 75;     // Porcentaje de humedad durante la noche

            // Calcular la variación horaria
            double humidityVariation = (morningHumidity - nightHumidity) / numPoints;

            for (int time = 0; time < numPoints; time++)
            {
                double timeFraction = (double)time / numPoints;

                // Interpolar entre los valores de humedad de diferentes momentos del día
                double interpolatedHumidity = InterpolateHumi(middayHumidity, afternoonHumidity, eveningHumidity, nightHumidity, timeFraction);

                // Agregar variación horaria
                interpolatedHumidity += humidityVariation * time;

                // Limitar a valores entre 0% y 100%
                interpolatedHumidity = Math.Max(0, Math.Min(100, interpolatedHumidity));

                // Añadir variación según el edificio
                if (Building == 1)
                {
                    interpolatedHumidity += 5; // Aumento de humedad para Building 1
                }
                else if (Building == 2)
                {
                    interpolatedHumidity -= 5; // Disminución de humedad para Building 2
                }
                else if (Building == 3)
                {
                    // No se hace ninguna modificación para Building 3
                }

                humidityValues[time] = Math.Round(interpolatedHumidity, 2); // Redondear a dos decimales
            }

            return humidityValues;
        }


        private static double InterpolateHumi(double midday, double afternoon, double evening, double night, double t)
        {
            if (t < 0.25)
                return midday + t * (afternoon - midday) * 4;
            else if (t < 0.5)
                return afternoon + (t - 0.25) * (evening - afternoon) * 4;
            else if (t < 0.75)
                return evening + (t - 0.5) * (night - evening) * 4;
            else
                return night + (t - 0.75) * (midday - night) * 4;
        }


        public static double[] Daylight_gen(int numPoints)
        {
            double[] daylightValues = new double[numPoints];

            // Definir un patrón de luz diurna típico (valores en lux)
            double[] daylightPattern = new double[]
            {
        100, 300, 600, 1200, 2000, 5000, 8000, 9000, 10000, 9000, 8000, 6000, 3000, 1000, 500
            };

            // Calcular el número de puntos en el patrón de luz
            int patternLength = daylightPattern.Length;

            for (int time = 0; time < numPoints; time++)
            {
                // Interpolar en el patrón de luz según la hora del día
                double timeFraction = (double)time / numPoints;
                int patternIndex = (int)(timeFraction * patternLength);
                double interpolatedDaylight = InterpolateLight(daylightPattern, patternIndex, timeFraction);

                daylightValues[time] = Math.Round(interpolatedDaylight, 2); // Redondear a dos decimales
            }

            return daylightValues;
        }

        private static double InterpolateLight(double[] values, int index, double t)
        {
            // Interpolación lineal entre dos valores en el patrón
            if (index < values.Length - 1)
            {
                double value1 = values[index];
                double value2 = values[index + 1];
                return value1 + (value2 - value1) * (t * values.Length - index);
            }
            else
            {
                return values[values.Length - 1];
            }
        }

        public static double[] AtmosphericPressure_gen(int Building, int numPoints, int floor, double[] daylightValues)
        {
            double[] pressureValues = new double[numPoints];

            // Definir rangos de presión atmosférica según el piso del edificio
            double basePressure = 1000; // Presión base para el primer piso
            double pressureChangePerFloor = 8; // Cambio de presión por cada piso adicional

            for (int time = 0; time < numPoints; time++)
            {
                // Obtener la presión base en función del piso del edificio
                double pressure = basePressure + floor * pressureChangePerFloor;

                // Ajustar la presión en función de la cantidad de luz
                pressure += 0.01 * daylightValues[time]; // Ajuste según la cantidad de luz (factor arbitrario)

                // Añadir algo de variación aleatoria
                Random random = new Random();
                double randomVariation = random.NextDouble() * 3 - 1.5; // Variación aleatoria entre -1.5 y 1.5 hPa
                pressure += randomVariation;

                // Asegurarse de que la presión esté en un rango realista
                pressure = Math.Max(950, Math.Min(1050, pressure));

                // Añadir variación según el edificio
                if (Building == 1)
                {
                    pressure += 5; // Aumento de presión para Building 1
                }
                else if (Building == 2)
                {
                    pressure -= 5; // Disminución de presión para Building 2
                }
                else if (Building == 3)
                {
                    // No se hace ninguna modificación para Building 3
                }

                pressureValues[time] = Math.Round(pressure, 2); // Redondear a dos decimales
            }

            return pressureValues;
        }

        public static double[] People_gen(int building, int floor, int numPoints, bool isWeekend)
        {
            double[] peopleValues = new double[numPoints];

            Random random = new Random();

            double basePeople, finalPeople;
            int buildingSize;

            // Definir el tamaño del edificio según los datos proporcionados
            switch (building)
            {
                case 1:
                    buildingSize = 1000;
                    break;
                case 2:
                    buildingSize = 2500;
                    break;
                case 3:
                    buildingSize = 3000;
                    break;
                default:
                    buildingSize = 1000; // Tamaño predeterminado en caso de edificio no especificado
                    break;
            }

            // Ajustar el número base de personas según la planta y el edificio
            switch (floor)
            {
                case 1:
                    basePeople = buildingSize * 0.5; // 5% del tamaño del edificio
                    break;
                case 2:
                    basePeople = buildingSize * 0.4; // 4% del tamaño del edificio
                    break;
                // Agregar más casos según sea necesario para cada planta
                default:
                    basePeople = buildingSize * 0.3; // Tamaño predeterminado en caso de planta no especificada
                    break;
            }

            for (int time = 0; time < numPoints; time++)
            {
                // Calcular la variación aleatoria entre -2 y 2 personas
                double randomVariation = random.Next(-2, 1);

                // Calcular la hora del día (en formato de 24 horas)
                int hourOfDay = (int)Math.Floor(((double)time / 60));

                // Ajustar el número de personas según la hora del día
                if ((hourOfDay >= 0 && hourOfDay < 8) || (hourOfDay >= 17))
                {
                    // Reducción durante la madrugada (0:00 AM a 8:00 AM) y la noche (5:00 PM a 0:00 AM)
                    finalPeople = basePeople * 0.1;
                }
                else if (hourOfDay >= 14 && hourOfDay < 15)
                {
                    // Reducción menos pronunciada durante la tarde (2:00 PM a 3:00 PM), almuerzo
                    finalPeople = basePeople * 0.6;
                }
                else
                {
                    finalPeople = basePeople;
                }

                // Aplicar la variación aleatoria y asegurarse de que no sea negativo
                peopleValues[time] = Math.Min(basePeople, Math.Max(0, Math.Ceiling(finalPeople + randomVariation)));
                
                if (building == 3 && floor == 2)
                {
                    if (hourOfDay >= 15 && hourOfDay < 16) 
                    {
                        peopleValues[time] += 2;
                    }
                }
            }

            // Ajustar el número de personas para el fin de semana
            if (isWeekend)
            {
                for (int i = 0; i < numPoints; i++)
                {
                    peopleValues[i] = Math.Max(0, Math.Ceiling(peopleValues[i] * 0.1)); // Reducción al 10% para los fines de semana
                }
            }

            return peopleValues;
        }


        public static double[] NoiseLevel_gen(int Building, int numPoints, double[] peopleValues, int floor)
        {
            double[] noiseValues = new double[numPoints];

            Random random = new Random();

            for (int time = 0; time < numPoints; time++)
            {
                double baseNoise = 50; // Nivel de ruido base en decibelios
                double noise;

                // Aumentar el ruido durante el día y en función del número de personas
                noise = baseNoise + (time < (5 * numPoints) / 8 ? (time / ((5 * numPoints) / 8)) * 10 : 0) + (peopleValues[time] / 100) * 20;

                // Añadir variación aleatoria
                double randomVariation = random.NextDouble() * 5 - 2.5; // Variación aleatoria entre -2.5 y 2.5 dB
                noise += randomVariation;

                // Asegurarse de que el nivel de ruido esté en un rango realista
                noise = Math.Max(40, Math.Min(80, noise));

                // Disminuir el ruido según la planta (mayor disminución para plantas superiores)
                noise -= floor * 1.5;

                // Añadir variación según el edificio
                if (Building == 1)
                {
                    noise += 5; // Aumento de ruido para Building 1
                }
                else if (Building == 2)
                {
                    noise -= 5; // Disminución de ruido para Building 2
                }
                else if (Building == 3)
                {
                    // No se hace ninguna modificación para Building 3
                }

                noiseValues[time] = Math.Round(noise, 2); // Redondear a dos decimales
            }

            return noiseValues;
        }


        public static double[] COCO2Level_gen(int Building, int numPoints)
        {
            double[] coCo2Values = new double[numPoints];

            // Generar valores de CO/CO2 aleatorios
            Random random = new Random();

            for (int time = 0; time < numPoints; time++)
            {
                double coCo2 = random.Next(18, 23); // Valores entre 18 y 23 (pueden ser ppm)

                // Añadir variación según el edificio
                if (Building == 1)
                {
                    coCo2 += 2; // Aumento de CO/CO2 para Building 1
                }
                else if (Building == 2)
                {
                    coCo2 -= 2; // Disminución de CO/CO2 para Building 2
                }
                else if (Building == 3)
                {
                    // No se hace ninguna modificación para Building 3
                }

                coCo2Values[time] = coCo2;
            }

            return coCo2Values;
        }


        public static double[] DayNight_gen(int numPoints)
        {
            double[] dayNightValues = new double[numPoints];

            for (int time = 0; time < numPoints; time++)
            {
                // Define un patrón de día y noche (por ejemplo, considera que es día de 6:00 AM a 8:00 PM)
                int sunriseTime = numPoints / 4;
                int sunsetTime = (3 * numPoints) / 4;

                // Comprueba si es día o noche en función de la hora
                if (time >= sunriseTime && time < sunsetTime)
                {
                    dayNightValues[time] = 1; // Es de día
                }
                else
                {
                    dayNightValues[time] = 0; // Es de noche
                }
            }

            return dayNightValues;
        }


    }


    //-----------------------------------------------------------------------------------------------------------------

    static void Main(string[] args)
    {

        string currentDate = "30/11/23";
        int numberData = 1440;

        List<Data> buildings = new List<Data>
        {
            new Data(1, currentDate),
            new Data(2, currentDate),
            new Data(3, currentDate)
        };

                List<List<Data>> buildingFloors = new List<List<Data>>
        {
            new List<Data>(),
            new List<Data>(),
            new List<Data>()
        };
        string connectionString = "Data Source=/Users/pol/Desktop/PAE/Programación/VISUAL STUDIO - PAE/DB.db";
        
        int[] floorCounts = { 5, 3, 7 };
            for (int buildingIndex = 0; buildingIndex < buildings.Count; buildingIndex++)
            {
                for (int floorIndex = 0; floorIndex <= floorCounts[buildingIndex]; floorIndex++)
                {
                    Console.WriteLine($"Processing Building {buildings[buildingIndex].Building}, Floor {floorIndex}");
                    double[] tempval;
                    double[] elec_val;
                    double[] water_val;
                    double[] gas_val;
                    double[] solarRad_val;
                    double[] solarPanels_val;
                    double[] aqiValues;
                    double[] humidityValues;
                    double[] daylightValues;
                    double[] pressureValues;
                    double[] peopleValues;
                    double[] coCo2Values;
                    double[] dayNightValues;
                    double[] noiseValues;
                    double[] peopleValuesWeekend;

                    if (floorIndex == 0)
                    {
                        tempval = Generation.Temperature_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData);
                        elec_val = Generation.Electricity_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData);
                        water_val = Generation.Water_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData);
                        gas_val = Generation.Gas_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData);
                        solarRad_val = Generation.SolarRadiation_gen(buildings[buildingIndex].Building, numberData);
                        solarPanels_val = Generation.SolarPanels_gen(numberData, solarRad_val);
                        aqiValues = Generation.AQI_gen(buildings[buildingIndex].Building, numberData);
                        humidityValues = Generation.Humidity_gen(buildings[buildingIndex].Building, numberData);
                        daylightValues = Generation.Daylight_gen(numberData);
                        peopleValues = Generation.People_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData, false);
                        peopleValuesWeekend = Generation.People_gen(buildings[buildingIndex].Building, buildings[buildingIndex].Floor, numberData, true);
                        coCo2Values = Generation.COCO2Level_gen(buildings[buildingIndex].Building, numberData);
                        dayNightValues = Generation.DayNight_gen(numberData);
                        pressureValues = Generation.AtmosphericPressure_gen(buildings[buildingIndex].Building, numberData, buildings[buildingIndex].Floor, daylightValues);
                        noiseValues = Generation.NoiseLevel_gen(buildings[buildingIndex].Building, numberData, peopleValues, buildings[buildingIndex].Floor);

                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 1, currentDate, tempval);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 2, currentDate, elec_val);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 3, currentDate, water_val);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 4, currentDate, gas_val);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 7, currentDate, solarRad_val);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 6, currentDate, solarPanels_val);
                        
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 8, currentDate, aqiValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 9, currentDate, humidityValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 13, currentDate, daylightValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 5, currentDate, peopleValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 12, currentDate, coCo2Values);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 16, currentDate, dayNightValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 10, currentDate, pressureValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 11, currentDate, noiseValues);
                        Functions.InsertRow_building(connectionString,buildings[buildingIndex].Building, 17, currentDate, peopleValuesWeekend);


                    }
                    else
                    {
                        aqiValues = Generation.AQI_gen(buildings[buildingIndex].Building, numberData);
                        humidityValues = Generation.Humidity_gen(buildings[buildingIndex].Building, numberData);
                        coCo2Values = Generation.COCO2Level_gen(buildings[buildingIndex].Building, numberData);
                        daylightValues = Generation.Daylight_gen(numberData);

                        peopleValues = Generation.People_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData, false);
                        peopleValuesWeekend = Generation.People_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData, true);

                        tempval = Generation.Temperature_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData);
                        elec_val = Generation.Electricity_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData);
                        water_val = Generation.Water_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData);
                        gas_val = Generation.Gas_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, buildingFloors[buildingIndex][floorIndex - 1].Floor, numberData);
                        solarRad_val = Generation.SolarRadiation_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, numberData);
                        pressureValues = Generation.AtmosphericPressure_gen(buildings[buildingIndex].Building, numberData, buildingFloors[buildingIndex][floorIndex - 1].Floor, daylightValues);
                        noiseValues = Generation.NoiseLevel_gen(buildingFloors[buildingIndex][floorIndex - 1].Building, numberData, peopleValues, buildingFloors[buildingIndex][floorIndex - 1].Floor);


                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 8, currentDate, aqiValues);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 9, currentDate, humidityValues);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 12, currentDate, coCo2Values);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 1, currentDate, tempval);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 2, currentDate, elec_val);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 3, currentDate, water_val);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 4, currentDate, gas_val);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 10, currentDate, pressureValues);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 11, currentDate, noiseValues);

                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 5, currentDate, peopleValues);
                        Functions.InsertRow_floor(connectionString,buildings[buildingIndex].Building,floorIndex, 17, currentDate, peopleValuesWeekend);


                    }

                    if (floorIndex < floorCounts[buildingIndex])
                    {
                        buildingFloors[buildingIndex].Add(new Data(buildings[buildingIndex].Building, floorIndex + 1, currentDate));
                    }
                }
            }
    }
}