
# DeviceDump

DeviceDump is a small little program that dumps the entire FileStream of a PhysicalDrive.

The intention behind it was I needed a quick access to read the raw bytes of a PhysicalDrive for investigation purposes, therefore I decided to make it public in case anyone else ever requires the same functioality.

Additionally any Drive I used still had to be available for other processes, meaning everytime DeviceDump quickly opens the part it requires and then immediately closes any linkage to the PhysicalDrive again, so that other processes are free to use the Drive again.

It reads 16 Bytes each row.

## How to use

When initially running the program, you will end up in this screen.

![Imgur](https://i.imgur.com/Zs0f5Gc.png)

Now obviously you might ask yourself, where can I now select my PhysicalDrive?

No worries! Simply click on *Start/Select Drive* and all the currently connected PhysicalDrives appear!

![Imgur](https://i.imgur.com/WGLYL6i.png)

Now you are free to simply click on the desired Drvie and all the magic happens by itself!

![Imgur](https://i.imgur.com/rpH6ziW.png)

And thats it! 

This is pretty much what DeviceDump does.
It gives you the *FileStream* in ```byte[]``` and ```string``` format, so that you are free to work with the data as desired.

### Additional Tools

In case you want to read more of the *FileStream* or quickly jump to a specific address, dont worry!

The functioality is given!

![Imgur](https://i.imgur.com/ZaNsfxg.png)

## Build yourself

If you wish to build this program yourself, you will require:

- Visual Studio 2022
- .NET 8.0 (net8.0-windows)

> To execute DeviceDump, you will require Administrator permissions on the system.

Once this requirements are met, simply fetch the master branch, open the project in Visual Studio and build the project.

![Imgur](https://i.imgur.com/CR1AwdV.png)

Afterwards, you are free to do whatever.


## License

This project is fully open-source and falls under the MIT License.

You are free to copy, modify and distribute the program to your liking.

Read more here [MIT License](LICENSE.txt).

