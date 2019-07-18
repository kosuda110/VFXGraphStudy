#!/bin/sh

### Set up astyle affected folders
BASE_DIR="../../../../.."
DIR="${BASE_DIR}/Assets/App"

init(){
  if [ ! -d ${BASE_DIR}/.git/hooks ]; then
      mkdir ${BASE_DIR}/.git/hooks
  fi
  cp -i $(pwd -P)/pre-commit.git ${BASE_DIR}/.git/hooks/pre-commit
  cp -i $(pwd -P)/.astylerc ${BASE_DIR}
  chmod 777 ${BASE_DIR}/.git/hooks/pre-commit
  echo "SUCCESS: File copied."
}

run(){
    echo "--- running astyle ---"
    for file in $(find ${DIR} -name "*.cs"); do
        format=true
        if $format; then
            astyle --options="$(pwd -P)/.astylerc" $file
        else
            echo "Skipping format for: $file"
        fi
    done
    echo "--- running astyle done ---"
}

clean(){
    for folder in $DIR; do
        find $folder -name "*.bak" -type f -delete
    done
}

case "$1" in
    init)
        init
        ;;
    run)
        run
        ;;
    clean)
        clean
        ;;
    *)
        echo "Usage: $0 {init|run|clean}"
esac

exit 0
