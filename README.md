Composer
========

Composer is a utility which can extract audio files from Halo 4 and invoke several helper utilities to automatically convert them to common formats.

Several "helper" programs are included with Composer that deal with converting the files once they are extracted. In fact, without them, Composer is just a simple PCK extractor. I do not take any credit for those programs whatsoever. See the Helpers.txt file for more information.

Also, I *highly* recommend that you do not redistribute the audio files produced by Composer or any of its helper utilities. Even if a file has been converted, it is still the property of 343 Industries and should be for personal use only.

Usage
-----

Halo 4's audio files and information are stored in two files on the disc named "soundbank.pck" and "soundstream.pck." Composer needs both of these files in order to accurately determine sound names and extraction information. At the very least, you will need to get these two files from the disc and place them into a folder.

Once Composer starts, you need to open both PCK files. You can choose to load both at once from a directory, or you can open them individually if they are separated or have different names.

After opening both files, you will be presented with a directory tree. From there, you can find files and use the buttons at the bottom of the window to extract them.

You may also set several options which control how files should be converted. You can disable conversion completely by unchecking the "Convert files after extraction" checkbox.

You can also control how xWMA files are converted by checking/unchecking the "Compress xWMA files" checkbox and by selecting a compression format. It is recommended that you compress these files or they will be exported as rather large WAV files. At the very least, you can use FLAC to compress the files without any loss of quality.

Supported Formats
-----------------

Composer is able to extract and convert from the following audio formats:
* Wwise OGG
* XMA
* xWMA and xWMA Pro (requires a 3rd-party utility; see below)

Unsupported files cannot be converted and will show up in the GUI as having a ".wem" extension. Most files should be convertible, however.

### Converting xWMA-Encoded Audio Files

Some of the game's audio files (most notably, the menu music) are compressed using the xWMA and xWMA Pro codecs. In order to convert these to common audio formats, Composer needs to make use of a utility known as "xWMAEncode.exe." The catch behind this is that the utility is included with Microsoft's DirectX SDK and cannot be legally redistributed on its own.

To obtain xWMAEncode.exe, you will need to download the June 2010 Microsoft DirectX SDK. A free download is available from microsoft.com here: http://www.microsoft.com/en-us/download/details.aspx?id=6812.

After downloading and installing the SDK, the program can be found in the Utilities\bin\x86 folder. If you do not wish to install the SDK, you can also just open the downloaded EXE in an archiving tool such as 7-zip and manually extract the program.

Once you have it, place xWMAEncode.exe in the "Helpers" folder and Composer will be able to recognize and use it.
