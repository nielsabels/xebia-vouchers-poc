# Overview

This is a proof of concept demonstration of a voucher system.

# Developing

Run all tests by executing the following command (using `cmd`, from the root directory).

```
powershell /c .\build.ps1 -Target=Test-Local
```

# Requirements

All logging will be forwarded to Seq, which is assumed to be running at [http://localhost:5341](http://localhost:5341). Running Seq is optional.

# Removing all containers

Warning: this will remove all containers.

In case there are lingering containers, breaking the `Cake` script (it won't be able to run the tests), use the following command to remove all your currently running containers. 

```
powershell /c docker rm $(docker ps -a -q) --force
```
