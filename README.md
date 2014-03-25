WELF.to.XML
===========

A windows commandline utility to convert the Trustwave WELF log files from WebMarshall proxy to XML.

Usage
-----

Simply put the WELF log files to be converted into the same directory as the application and then run the application.

It will attempt to convert all files with a .log extension to XML.  If there are multiple files it will conert them in parallel, therefore if it is processing a number of large files it can use a lot of system resources - so don't run it on the proxy server itself!

Known Issues
------------

* The application has no error checking or exception handling.  
* It requires the user to have read/write/create permissions on the directory that it is being run from.
