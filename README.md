# LabelWare
LabelWare is a command-line software that allows you to copy labels from one repository to others easily and without need to manually modify JSON files or inputting all the repository names into a huge file. 

## How to use
1. Download and run LabelWare.
2. Using interactive command-line interface, input your Personal Access Token that has sufficient permissions to read and modify labels of all repositories that you want to modify.
3. Select source repository from which labels will be copied.
4. Select target repositories to which labels will be pasted.
5. Begin operation.

## Effects on target repositories
- If a label exists that has matching name (case-insensitively), it will be edited to have new color, description and correct case.
- Labels that do not exist in source repository will be deleted.
- Labels that are missing will be created.
