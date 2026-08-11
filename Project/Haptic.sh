mkdir -p Haptic.app/Contents/MacOS
mkdir -p Haptic.app/Contents/Resources

cp Haptic.exe Haptic.app/Contents/Resources/

cat << 'EOF' > Haptic.app/Contents/MacOS/Haptic
#!/bin/bash
APP_DIR="$(cd "$(dirname "$0")"/../Resources && pwd)"
osascript -e "tell application \"Terminal\" to do script \"mono '$APP_DIR/Haptic.exe'\""
EOF

chmod +x Haptic.app/Contents/MacOS/Haptic