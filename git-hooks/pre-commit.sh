#!/bin/sh

if git rev-parse --verify HEAD >/dev/null 2>&1
then
	against=HEAD
else
	# Initial commit: diff against an empty tree object
	against=4b825dc642cb6eb9a060e54bf8d69288fbee4904
fi

repoRoot=`git rev-parse --show-toplevel`

echo "JS files:"

stagedFiles=`git diff-index --name-only --diff-filter=AM ${against} -- `
arrFiles=(${stagedFiles//\r?\n|\r/})
runLint=false

for i in "${arrFiles[@]}"
do
	case "$i" in
		(*"vendor"*".js") ;;
		(*".js") runLint=true; `node node_modules/js-beautify/js/bin/js-beautify -f $i -r`; `git add $i`; echo $i;;
	esac
done

if $runLint
then
	grunt jshint
	if [ $? != 0 ]
		then
		exit 1
	fi
fi
