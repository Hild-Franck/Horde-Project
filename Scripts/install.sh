#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
echo 'Downloading from https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorInstaller/Unity-2017.3.1f1.pkg: '
curl -o Unity.pkg https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorInstaller/Unity-2017.3.1f1.pkg

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /

echo 'Downloading from https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.3.1f1.pkg: '
curl -o MacSupport.pkg https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.3.1f1.pkg

echo 'Installing MacSupport.pkg'
sudo installer -dumplog -package MacSupport.pkg -target /
