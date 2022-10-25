using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

My_Copy M = new();
M.CreateSettings();
M.LoadSettings();
M.CreateFolder();
M.CopyFiles();
M.ArchiveFiles();
Console.WriteLine("SourcePath: " + M.pathSource + "\nTargetPath: " + M.pathTarget);
Console.WriteLine(M.log);
M.CreateLogFolder();
M.SaveLog();
Console.WriteLine("\nPress any key to continue.");
Console.Read();
M.Del();
class My_Copy
{
    public string log;
    string? reserv = "\\Reserv";
    public string? pathSource { get; set; }
    public string? pathTarget { get; set; }
    public async void CreateSettings()
    {
        try
        {
            using (FileStream f = new("Settings_Folder.json", FileMode.Create))
            {
                Settings_Folder folder = new("D:\\Test", "D:\\Test\\TempTEST");
                await JsonSerializer.SerializeAsync<Settings_Folder>(f, folder);
                //Console.WriteLine("Файл создан.\n");
            }
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
    }
    public void LoadSettings()
    {
        try
        {
            using (FileStream f2 = new("Settings_Folder.json", FileMode.Open))
            {
                Settings_Folder? folder = JsonSerializer.Deserialize<Settings_Folder>(f2);
                pathSource = folder.Source;
                pathTarget = folder.Target;
                Console.WriteLine("Файл открыт.\n");
            }
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
    }
    public void CreateFolder()
    {
        Directory.CreateDirectory(pathSource + reserv);
    }
    public void ArchiveFiles()
    {
        Directory.CreateDirectory(pathTarget);
        File.Delete(pathTarget + "\\Archive.zip");
        try
        {
            ZipFile.CreateFromDirectory(pathSource + reserv, pathTarget + "\\Archive.zip");
            
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
    }
    public void CopyFiles()
    {
        try
        {
            foreach (var fi in new DirectoryInfo(pathSource).EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                if (fi.Exists)
                    File.Copy(fi.FullName, pathSource + reserv + "\\" + fi.Name, true);
                log += fi.FullName + "\n";
            }
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
    }
    public void CreateLogFolder()
    {
        Directory.CreateDirectory(pathSource + "\\Log");
        //File.Create(pathSource + "\\Log" + "\\Log.txt");
    }
    public void SaveLog()
    {
        try
        {
            StreamWriter sw = new StreamWriter(File.Open(pathSource + "\\Log\\Log.txt", FileMode.Append)); // эта запись в файл не глючит + создает файл, если нужно
            sw.WriteLine(log);
            sw.Close();
        }
        catch (Exception e) { Console.WriteLine(e.Message); }

    }
    public void Del()
    {
        Directory.Delete(pathSource + reserv, true);
    }
    ~My_Copy()
    {
        Console.WriteLine("Destructor");
        Del();
    }
}
class Settings_Folder
{
    public string Source { get; set; }
    public string Target { get; set; }
    public Settings_Folder(string source, string target)
    {
        Source = source;
        Target = target;
    }
}