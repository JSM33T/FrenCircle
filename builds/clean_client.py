import os
import shutil

def delete_folders(*paths):
    # Navigate to the directory where the script is running
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)

    for path in paths:
        # Resolve the absolute path for each folder
        folder_path = os.path.abspath(path)

        # Check if the folder exists
        if os.path.isdir(folder_path):
            try:
                # Delete the folder
                shutil.rmtree(folder_path)
                print(f"Deleted folder: {folder_path}")
            except Exception as e:
                print(f"Failed to delete folder '{folder_path}': {e}")
        #else:
            #print(f"Folder '{folder_path}' does not exist.")

# Example usage
delete_folders('../client/dist', '../api/API.Base/obj', '../api/API.Base/bin','../api/API.Entities/bin','../api/API.Entities/obj','../api/API.Infra/bin','../api/API.Infra/obj','../api/API.Repositories/bin','../api/API.Repositories/obj','../api/API.Validators/bin','../api/API.Validators/obj'                                                             )