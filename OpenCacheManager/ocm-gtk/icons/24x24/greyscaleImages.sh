#!/bin/bash

# remove old greyscaled Images:
rm disabled-*.png
rm archived-*.png

for file in *.png; do
    convert $file -colorspace GRAY disabled-$file
    convert $file -colorize 00,50,50 archived-$file
done

