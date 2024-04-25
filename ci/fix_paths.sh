if [ -d "Coverage/unity" ]; then
    find Coverage/unity -name "*.xml" -type f -print0 | xargs -0 -I {} sh -c '
        echo "Fixing paths in {}"
        sed -i "s/\/github\/workspace\/Unity\/Packages\/se.hertzole.gamejolt.net\/Runtime/GameJolt.NET/g" "{}"
    '
fi

if [ -d "TestResults/net" ]; then
    find TestResults/net -name "*.xml" -type f -print0 | xargs -0 -I {} sh -c '
        echo "Fixing .NET paths in {}"
        sed -i "s/\/home\/runner\/work\/gamejolt.net\/gamejolt.net\///g" "{}"
    '
fi