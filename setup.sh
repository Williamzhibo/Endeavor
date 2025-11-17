#!/bin/bash

# This script sets up the development environment for the Endeavor game on macOS.

# Check for Homebrew and install if not found
if ! command -v brew &> /dev/null; then
    echo "Homebrew not found. Installing Homebrew..."
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
fi

# Install SDL2 libraries
echo "Installing SDL2 libraries..."
brew install sdl2 sdl2_image sdl2_mixer sdl2_ttf

echo "Setup complete. You can now build and run the project using 'dotnet run'."
