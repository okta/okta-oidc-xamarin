#!/bin/bash

echo `date` > ./tickle

git commit -am 'tickle build'
git push azure
