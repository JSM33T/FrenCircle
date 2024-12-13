import os

def combine_sql_scripts(directory, output_file):
    # Navigate to the directory where the script is running
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)

    # Resolve the absolute path for the target directory
    directory = os.path.abspath(directory)

    # Ensure the directory exists
    if not os.path.isdir(directory):
        print(f"Directory '{directory}' does not exist.")
        return

    # Get all SQL files in the specified directory
    sql_files = [f for f in os.listdir(directory) if f.endswith('.sql')]

    if not sql_files:
        print(f"No SQL files found in the directory '{directory}'.")
        return

    with open(output_file, 'w', encoding='utf-8') as outfile:
        for sql_file in sql_files:
            file_path = os.path.join(directory, sql_file)
            with open(file_path, 'r', encoding='utf-8') as infile:
                outfile.write(infile.read())
                outfile.write('\nGO\n')  # Add "GO" after each script

    print(f"Combined script saved to {output_file}")

# Specify the directory and output file
combine_sql_scripts(directory='../sql/tables', output_file='../sql/master/tables.sql')
combine_sql_scripts(directory='../sql/sprocs', output_file='../sql/master/sprocs.sql')