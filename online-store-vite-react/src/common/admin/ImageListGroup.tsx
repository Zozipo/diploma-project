import { FC, InputHTMLAttributes } from "react";
import { FormikErrors, FormikTouched } from "formik";
import { FaTrash } from "react-icons/fa";

interface ImageListGroupProps extends InputHTMLAttributes<HTMLInputElement> {
  images: File[];
  error?: string | string[] | FormikErrors<File>[] | undefined;
  touched?: boolean | FormikTouched<File>[] | undefined;
  setFieldValue: (field: string, value: File[]) => void;
}

const ImageListGroup: FC<ImageListGroupProps> = ({
  error,
  touched,
  images,
  setFieldValue,
}) => {
  const handleDelete = (index: number) => {
    const newImages = images.filter((_, i) => i !== index);
    setFieldValue("images", newImages);
  };

  return (
    <>
      <div className="flex justify-between gap-5 flex-wrap mb-6">
        {images.map((file, index) => (
          <div
            className="relative w-80 h-80 border border-gray-300 bg-gray-50 rounded-lg overflow-hidden flex items-center justify-center"
            key={index}
          >
            <img
              src={URL.createObjectURL(file)}
              alt={`Uploaded ${index}`}
              className="object-contain w-full h-full p-3"
            />
            <button
              type="button"
              onClick={() => handleDelete(index)}
              className="absolute top-2 right-2 text-red-600 bg-white rounded-full p-2 shadow-lg "
            >
              <FaTrash size={20} />
            </button>
          </div>
        ))}
      </div>

      <div className="mb-6 items-center justify-center w-full">
        <label
          htmlFor="dropzone-file"
          className={`flex flex-col items-center justify-center w-full h-64 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 ${
            error && touched
              ? "border-red-500 dark:border-red-400 bg-red-50"
              : "dark:hover:bg-bray-800 dark:bg-gray-700 hover:bg-gray-100 dark:hover:border-gray-500 dark:hover:bg-gray-600"
          }`}
        >
          <div className="flex flex-col items-center justify-center pt-5 pb-6">
            <svg
              className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 20 16"
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
              />
            </svg>
            <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
              <span className="font-semibold">Click to upload</span> or drag and
              drop
            </p>
            <p className="text-xs text-gray-500 dark:text-gray-400">
              SVG, PNG, JPG
            </p>
          </div>
        </label>
        <input
          id="dropzone-file"
          type="file"
          className="hidden"
          name="images"
          accept="image/*"
          multiple
          onChange={(event) => {
            const files = event.currentTarget.files;
            if (files) {
              const newImages = [...images, ...Array.from(files)];
              setFieldValue("images", newImages);
            }
          }}
        />
        {error && touched && (
          <div className="mt-2 text-sm text-red-600 dark:text-red-500">
            {error.toString()}
          </div>
        )}
      </div>
    </>
  );
};

export default ImageListGroup;
