# PAE-Accenture
Repository to contain all the code regarding the PAE-Accenture 

## Important information to be aware of

### How to use .gitignore file in local workspace
  * This file exists so you as a self developer make yourself responsible of uploading to GitHub (so here) only the code that is useful to others, code that doesn't retain this criteria should not be uploaded to this repo, so you must add those necessary files/folders to the .gitignore file. Knowledge on how to use the .gitignore file can be found over at this link: [Learn more about .gitignore](https://git-scm.com/docs/gitignore)
If you don't like to resource I've linked, feel free to use any other on the internet.

### How to clone into local repo only necessary files (super useful & important!)
  * This repository is getting large, and so if you don't want to have all the files downloaded & installed on your computer, and only want to work on a sub-set of the repo's files, follow along with the following commands to learn how to perform what is called a "git check-out" action. In order to perform this action, empty your local respository (delete all files/folders inside as well as the .git directory (with a `rm -fr .git` command on Mac, for the rest of system look it up yourself)). Once the working directory is clean, perform the following actions:

    [1] `git clone --filter=blob:none --no-checkout --depth 1 https://github.com/Shumbabala/PAE-Accenture.git .`
    
    (Note that the "--depth 1" option is only for commit history download purpouses. ALso don't forget the "." at the end!)
    
    [2] `git sparse-checkout init --cone`
    
    [3] `git sparse-checkout set <path/to/files/or/folders/you/want>`
    
    [4] `git checkout main` (or instead of "main" you can use whatever name you want for your prinicipal local workspace branch)

    -> Once these commands are done, you will be able to work on your PAE project, only having present in your local computer the files that are of interest to you. Any questions make sure to forward them to the PAE_Accenture WhatsApp group.

### How to FETCH + COMPARE branches before pushing to remote

Here is explained how to make sure that when you push your work upstream to remote, you don't overwrite changes that others committed. Understand that we're all working on possibly the same folders/files and if everyone just force pushes their work upstream, we'll just overwrite each other and have nothing accomplished. So when you start your day and want to work on a project aspect, do the following:

 [1] Open up VSCode or whatever your working IDE is and direct yourself to the working directory where you have your git repository installed.
 
 [2] Inside your git repository now, do a `git fetch`command (to download the updated content from origin into your remote tracking branches.
 
 [3]  Make sure you're located on your main branch at this moment, to check you can do `git branch` and it will output whatever working branch you are in right now. Let's suppose your working branch is called "main" for the remainder of this explanation. You need to remember this name for the remainder of the steps.
 
 [4] Try doing a `git merge`which will try to update your local branch with the remote's. If the action fails, it's probably because you need to need to save and commit your changes, try to following the on-screen instructions shown to you. 
 
 [5] Once you've managed to update your local repository with the remote (make sure you're at this stage, you can check by running `git status`and it should tell you that your branch is updated or in synch with the remote) do `git checkout -b <Whatever-you-want-to-call-your-local-support-branch> <main (or whatever your main working branch is as explained in step [3])>`, with this command you've created a sibling branch with the same contents as your "main" branch (created a copy). You will use this copied branch to perform your work and changes, run experimental code etc. Since it's a seperated branch from your main, you can experiment with it as you like.
 
 [6] Verify that you're in the newly created branch with `git branch`which should output the branch you've just created in the previous step. You can now do your work, once you're done, commit as usual with `git commit -m "random message"`.
 
 [7] Once you're finished working on your experimental branch and want to commit your changes to the main branch (to later push to remote), do `git checkout <whatever your main branch is>`followed by`git merge --no-ff <whatever you called your local support branch>`. This should merge your main branch with your support branch.
 
 [8] Now push (`git push`or `git push origin`, whichever works). If it fails it's because probably some commits have been made to the remote, you must pull them to your local before pushing (resolve merge conflicts etc).
 
 [9] You have succesfully pushed and updated code in the remote GitHub repository without interfering or overwriting anyone else's work, congratulations! Any problems you run into let them be known in the group chat of course.


## Any further important data that could be of use to others, let the project manager know (Gerard) and he'll make sure to add it here
