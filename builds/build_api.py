import os
import subprocess

def build_dotnet_project():
    # Navigate to the directory where the script is running
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)

    # Define the target directory (relative to the script's directory)
    target_directory = os.path.abspath('../api/API.Base')

    # Ensure the directory exists
    if not os.path.isdir(target_directory):
        print(f"Directory '{target_directory}' does not exist.")
        return

    # Navigate to the target directory
    os.chdir(target_directory)

    # Run the 'dotnet build' command
    try:
        subprocess.run(['dotnet', 'build'], check=True)
        print("Build completed successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Build failed with error: {e}")

# Execute the function
build_dotnet_project()