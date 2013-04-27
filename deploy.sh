
rm -rf ../tinyleap-ghpages/*
mv webplayer/webplayer.html webplayer/index.html
cp webplayer/* ../tinyleap-ghpages/.

cd ../tinyleap-ghpages
git add -A
git commit -m "new build"
git push origin gh-pages

cd ../tinyleap
