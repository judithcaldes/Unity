/*using System.Collections;
using System.IO;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private const string databaseName = "FinalDB.db";
    private SQLiteConnection _connection;

    private IEnumerator Start()
    {
        yield return StartCoroutine(InitializeDatabase());
    }

    private IEnumerator InitializeDatabase()
    {
        // El archivo se almacenará en la carpeta persistente de la aplicación.
        
        string destinationPath = Path.Combine(Application.persistentDataPath, databaseName);
        //string destinationPath = "jar:file://" + Application.dataPath + "!/assets/" + databaseName;

        // Si la base de datos no existe en la ruta de destino, debemos copiarla de StreamingAssets
        if (!File.Exists(destinationPath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, databaseName);

            // En Android, debemos usar UnityWebRequest para acceder a los archivos en StreamingAssets
            if (Application.platform == RuntimePlatform.Android)
            {
                UnityWebRequest www = UnityWebRequest.Get(sourcePath);
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error downloading the database: {www.error}");
                    yield break;
                }

                // Guardamos los datos obtenidos en la ruta de destino
                File.WriteAllBytes(destinationPath, www.downloadHandler.data);
                Debug.Log("Database file copied to: " + destinationPath);
            }
            else // Para otras plataformas
            {
                // Directamente copiamos el archivo desde StreamingAssets a la ruta de destino
                File.Copy(sourcePath, destinationPath, true);
                Debug.Log("Database file copied to: " + destinationPath);
            }
        }

        // Ahora conectamos a la base de datos en la ruta de destino (ya no usamos URI=file:)
        _connection = new SQLiteConnection(destinationPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Connected to database at path: " + destinationPath);
    }

    public SQLiteConnection GetConnection()
    {
        return _connection;
    }

    private void OnApplicationQuit()
    {
        _connection?.Close();
    }
}
*/
