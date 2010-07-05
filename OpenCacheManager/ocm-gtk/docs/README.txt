Here's the correct steps to build OCM for Ubuntu 9.04/9.10. 
It won't work on Ubuntu 10.04 yet (different version of Webkit, the browser used to show a map),
and the steps are different for RPM based distributions like Fedora. For those distributions, try the 
binary package

Get Additional Dependencies
This only needs to be done once
1) Open a terminal window
2) Type "sudo apt-get install mono-gmcs"
3) Type "sudo apt-get install libwebkit1.0-cil"
4) Type "sudo apt-get install libgtkhtml3.16-cil"
5) Type "sudo apt-get install gpsbabel"

Build and Install
1) Get the latest Source package from the project site (http://sourceforge.net/projects/opencachemanage/)
2) Extract the file to your home directory or another location
3) Open a terminal window
4) CD into the directory where you extracted the above
5) Type "./configure"
6) Type "sudo make install"

Once that is complete, you should see "Open Cache Manager" in your Applications menu under "Internet"