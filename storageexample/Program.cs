namespace storageexample;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Azure Blob Storage Exercise");




        await Process();

    }


    static async Task Process()
    {
        Console.WriteLine("process method");


        DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions()
        {
            ExcludeEnvironmentCredential = true,
            ExcludeManagedIdentityCredential = true
        };

        string accountName = "randoaccountone";
        DefaultAzureCredential credential = new DefaultAzureCredential(options);

        // CREATE A BLOB STORAGE CLIENT

        string blobServiceEndpoint = $"https://{accountName}.blob.core.windows.net";
        BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), credential);

        // CREATE A CONTAINER

        string containerName = "wtblob" + Guid.NewGuid().ToString();
        Console.WriteLine("Creating the container: " + containerName);

        BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);


        //check if container was created 

        if (containerClient is null)
        {
            Console.WriteLine("Container was created successfully, press 'enter' to continue.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Container was created successfully, press 'enter' to continue.");
            Console.ReadKey();
        }

        // CREATE A LOCAL FILE FOR UPLOAD TO BLOB STORAGE

        Console.WriteLine("Creating a local file for upload to Blob storage...");
        string localPath = "./data/";
        string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
        string localFilePath = Path.Combine(localPath, fileName);


        // Write text to the file
        await File.WriteAllTextAsync(localFilePath, "Hello, World!");
        Console.WriteLine("Local file created, press 'Enter' to continue.");
        Console.ReadLine();

        // UPLOAD THE FILE TO BLOB STORAGE

        // Get a reference to the blob and upload the file
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}", blobClient.Uri);

        // Open the file and upload its data
        using (FileStream uploadFileStream = File.OpenRead(localFilePath))
        {
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();
        }

        // Verify if the file was uploaded successfully
        bool blobExists = await blobClient.ExistsAsync();
        if (blobExists)
        {
            Console.WriteLine("File uploaded successfully, press 'Enter' to continue.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("File upload failed, exiting program..");
            return;
        }

        // LIST BLOBS IN THE CONTAINER

        Console.WriteLine("Listing blobs in container...");
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine("\t" + blobItem.Name);
        }

        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();

        // DOWNLOAD THE BLOB TO A LOCAL FILE

        // overwrite the original file

        string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

        Console.WriteLine("Downloading blob to: {0}", downloadFilePath);

        // Download the blob's contents and save it to a file
        BlobDownloadInfo download = await blobClient.DownloadAsync();

        using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
        {
            await download.Content.CopyToAsync(downloadFileStream);
        }

        Console.WriteLine("Blob downloaded successfully to: {0}", downloadFilePath);
    }
}
