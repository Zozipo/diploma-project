import { useNavigate, useParams } from "react-router-dom";
import * as Yup from "yup";
import { useEffect, useState } from "react";
import http_common from "../../../../../http_common.ts";
import { ICategoryUpdate } from "../../../../../entities/Category.ts";
import { ICategory } from "../../../../../entities/Category.ts";
import { Form, Formik } from "formik";
import InputGroup from "../../../../../common/admin/InputGroup.tsx";
import ImageGroup from "../../../../../common/admin/ImageGroup.tsx";
import MultilevelDropdownGroup from "../../../../../common/admin/MultilevelDropdownGroup.tsx";
import { toast } from "react-toastify";
import { IoClose } from "react-icons/io5";
import EditorTiny from "../../../../../common/admin/EditorTiny.tsx";

function CategoriesUpdatePage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [allCategories, setAllCategories] = useState<ICategory[]>([]);

  const [initialValues, setInitialValues] = useState<ICategoryUpdate>({
    name: "",
    description: "",
    parentCategoryId: null,
    image: null,
  });

  const categorySchema = Yup.object().shape({
    description: Yup.string()
      .required("Description is required")
      .max(255, "Description must be smaller"),
    name: Yup.string()
      .required("Name is required")
      .min(3, "Name must be at least 3 characters")
      .max(255, "Name must be smaller"),
    image: Yup.mixed().required("Image is required"),
  });

  const fetchCategories = async () => {
    await http_common
      .get<ICategory[]>("api/Categories")
      .then((resp) => {
        setAllCategories(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to fetch categories data");
        console.error("Error fetching categories data:", error);
      });
  };

  const fetchCategory = async () => {
    http_common
      .get(`api/Categories/${id}`)
      .then(async (resp) => {
        setInitialValues((prevValues) => ({
          ...prevValues,
          name: resp.data.name,
          description: resp.data.description,
          parentCategoryId: resp.data.parentCategoryId,
          image: null,
        }));

        const imageResponse = await http_common.get(
          `/images/1200_${resp.data.image}`,
          { responseType: "arraybuffer" },
        );

        const imageBlob = new Blob([imageResponse.data], {
          type: "image/webp",
        });
        const imageFile = new File([imageBlob], resp.data.image, {
          type: "image/webp",
        });

        setInitialValues((prevValues) => ({
          ...prevValues,
          image: imageFile,
        }));
      })
      .catch((error) => {
        toast.error("Failed to fetch category data");
        console.error("Error fetching category data:", error);
        navigate(-1);
      });
  };

  useEffect(() => {
    window.scrollTo({ top: 0 });
    fetchCategories();
    fetchCategory();
  }, [id]);

  const handleSubmit = async (values: ICategoryUpdate) => {
    await categorySchema.validate(values);

    await http_common
      .put(`api/Categories/${id}`, values, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then(() => {
        toast.success("Category updated successfully");
      })
      .catch((error) => {
        toast.error("Failed to update category");
        console.error("Error editing category:", error);
      });

    navigate(-1);
  };

  return (
    <>
      <div className="p-5 sm:ml-64">
        <Formik
          initialValues={initialValues}
          onSubmit={handleSubmit}
          validationSchema={categorySchema}
          enableReinitialize={true}
        >
          {({
            handleChange,
            values,
            errors,
            touched,
            handleBlur,
            setFieldValue,
          }) => (
            <Form className="m-5">
              <div className="flex justify-between">
                <p className="text-xl text-gray-900 dark:text-white">
                  Update Category
                </p>
                <button
                  onClick={() => navigate(-1)}
                  type="button"
                  className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
                  data-modal-hide="default-modal"
                >
                  <IoClose size={26} />
                </button>
              </div>
              <hr className="h-px my-8 bg-gray-200 border-0 dark:bg-gray-700" />
              <MultilevelDropdownGroup
                label="Select parent category"
                items={allCategories}
                selectedItem={values.parentCategoryId}
                error={errors.parentCategoryId}
                touched={touched.parentCategoryId}
                setFieldValue={setFieldValue}
              ></MultilevelDropdownGroup>
              <ImageGroup
                image={values.image}
                setFieldValue={setFieldValue}
                error={errors.image}
                touched={touched.image}
              ></ImageGroup>
              <InputGroup
                label="Name"
                type="text"
                field="name"
                placeholder="Enter name"
                handleBlur={handleBlur}
                error={errors.name}
                touched={touched.name}
                handleChange={handleChange}
                value={values.name}
              ></InputGroup>
              <EditorTiny
                value={values.description}
                label="Description"
                error={errors.description}
                touched={touched.description}
                onEditorChange={(text: string) => {
                  setFieldValue("description", text);
                }}
              />
              <button
                type="submit"
                className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
              >
                Save
              </button>
              <button
                onClick={() => navigate(-1)}
                data-modal-hide="default-modal"
                type="button"
                className="py-2.5 px-5 ms-3 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
              >
                Cancel
              </button>
            </Form>
          )}
        </Formik>
      </div>
    </>
  );
}

export default CategoriesUpdatePage;
