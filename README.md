# ProjectSorter

Can be used to sort pictures quickly

## Installation

Intended to be used with the latest Ubuntu.

Uses jpegtran and dotnet. So do:

    sudo apt install libjpeg-turbo-progs dotnet-sdk-8.0

Do

    cd ~
    git clone https://github.com/rominator1983/PictureSorter

    cd ~/PictureSorter

    dotnet publish --configuration Release /p:PublishProfile=standard

As long as there is no dpkg add a file with the following content to `~/.local/share/applications`

    [Desktop Entry]
    Exec=/home/REPLACE WITH USER/PictureSorter/PictureSorter/bin/Release/net8.0/linux-x64/publish/PictureSorter %U
    Name=Picture Sorter
    Terminal=false
    Type=Application

## Usage

Open Pictures in Nautilus with Picture Sorter and press F1 for more info.
