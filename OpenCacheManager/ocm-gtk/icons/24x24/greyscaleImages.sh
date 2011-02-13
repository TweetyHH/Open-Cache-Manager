#!/bin/bash

# remove old greyscaled Images:
rm disabled-*.png
rm archived-*.png

for file in *.png; do
    convert $file -colorspace GRAY archived-$file
    convert $file -colorize 25,25,25 disabled-$file
done

