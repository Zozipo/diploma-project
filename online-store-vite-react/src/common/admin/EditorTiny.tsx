import { Editor, IAllProps } from "@tinymce/tinymce-react";
import classNames from "classnames";
import { FC } from "react";

interface IEditorProps extends IAllProps {
  label: string;
  error?: string | undefined;
  touched?: boolean | undefined;
}

const EditorTiny: FC<IEditorProps> = ({ label, error, touched, ...props }) => {
  return (
    <div className="mb-3">
      {label && (
        <label
          htmlFor="input-group-1"
          className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        >
          {label}
        </label>
      )}
      <div
        className={classNames(
          "form-control",
          { "is-invalid border border-4 border-danger": touched && error },
          { "is-valid border border-4 border-success": touched && !error },
        )}
      >
        <Editor
          apiKey="in3xle77outg3zgdm6x7yekn3wq1ho947eaeimlrup2t03eu"
          init={{
            height: 500,
            language: "en",
            menubar: true,
            images_file_types: "jpg,jpeg",
            block_unsupported_drop: false,
            menu: {
              file: {
                title: "File",
                items: "newdocument restoredraft | preview | print ",
              },
              edit: {
                title: "Edit",
                items: "undo redo | cut copy paste | selectall | searchreplace",
              },
              view: {
                title: "View",
                items:
                  "code | visualaid visualchars visualblocks | spellchecker | preview fullscreen",
              },
              insert: {
                title: "Insert",
                items:
                  "image link media template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor toc | insertdatetime",
              },
              format: {
                title: "Format",
                items:
                  "bold italic underline strikethrough superscript subscript codeformat | formats blockformats fontformats fontsizes align lineheight | forecolor backcolor | removeformat",
              },
              tools: {
                title: "Tools",
                items: "spellchecker spellcheckerlanguage | code wordcount",
              },
              table: {
                title: "Table",
                items: "inserttable | cell row column | tableprops deletetable",
              },
              help: { title: "Help", items: "help" },
            },
            plugins: [
              "advlist autolink lists link image imagetools charmap print preview anchor",
              "searchreplace visualblocks code fullscreen textcolor ",
              "insertdatetime media table paste code help wordcount",
            ],
            toolbar:
              "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | forecolor backcolor",
            content_langs: [
              { title: "English", code: "en" },
              { title: "Українська", code: "ua" },
            ],
            content_style:
              "body { font-family:Helvetica,Arial,sans-serif; font-size:14px }",
          }}
          {...props}
        />
      </div>
      {touched && error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};

export default EditorTiny;
