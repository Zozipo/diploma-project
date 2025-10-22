import { useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { useEffect, useState } from "react";
import http_common from "../../../../../http_common.ts";
import { IProductCreate } from "../../../../../entities/Product.ts";
import { Form, Formik } from "formik";
import InputGroup from "../../../../../common/admin/InputGroup.tsx";
import { ICategory } from "../../../../../entities/Category.ts";
import ImageListGroup from "../../../../../common/admin/ImageListGroup.tsx";
import SelectGroup from "../../../../../common/admin/SelectGroup.tsx";
import CheckboxGroup from "../../../../../common/admin/CheckboxGroup.tsx";
import { toast } from "react-toastify";
import { IoClose } from "react-icons/io5";
import EditorTiny from "../../../../../common/admin/EditorTiny.tsx";

function ProductsCreatePage() {
  const navigate = useNavigate();

  const [categories, setCategories] = useState<ICategory[]>([]);

  const [initialValues] = useState<IProductCreate>({
    title: "",
    description: "",
    price: 0,
    images: [],
    rating: 0,
    discount: 0,
    categoryId: 0,
    deliveryKit: "",
    isStock: false,
  });

  const productSchema = Yup.object().shape({
    description: Yup.string()
      .required("Description is required")
      .max(255, "Description must be smaller"),
    title: Yup.string()
      .required("Name is required")
      .min(3, "Name must be at least 3 characters")
      .max(255, "Name must be smaller"),
    price: Yup.number().required("Price is required").min(0, "Price min is 0"),
    discount: Yup.number()
      .lessThan(Yup.ref("price"), "Discount must be lower than the price")
      .nullable()
      .min(0, "Discount min is 0"),
    images: Yup.array()
      .required("At least one image is required")
      .min(1, "At least one image is required"),
    deliveryKit: Yup.string()
      .required("DeliveryKit is required")
      .max(255, "DeliveryKit must be smaller"),
  });

  useEffect(() => {
    http_common
      .get<ICategory[]>("api/Categories")
      .then((resp) => {
        setCategories(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to fetch categories data");
        console.error("Error fetching categories data:", error);
      });
  }, []);

  const handleSubmit = async (values: IProductCreate) => {
    await productSchema.validate(values);

    const formData = new FormData();

    Object.entries(values).forEach(([key, value]) => {
      if (key === "images") {
        (value as File[]).forEach((file: File) => {
          formData.append(key, file);
        });
      } else {
        const sanitizedValue = value === "" ? null : value;
        formData.append(key, sanitizedValue?.toString() ?? "");
      }
    });

    await http_common
      .post("api/Products", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then(() => {
        toast.success("Product created successfully");
      })
      .catch((error) => {
        toast.error("Failed to create product");
        console.error("Error creating product:", error);
      });

    navigate(-1);
  };

  return (
    <>
      <div className="p-5 sm:ml-64">
        <Formik
          initialValues={initialValues}
          onSubmit={handleSubmit}
          validationSchema={productSchema}
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
                  Create Product
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
              <ImageListGroup
                images={values.images}
                setFieldValue={setFieldValue}
                error={errors.images}
                touched={touched.images}
              ></ImageListGroup>
              <InputGroup
                label="Title"
                type="text"
                field="title"
                placeholder="Enter title"
                handleBlur={handleBlur}
                error={errors.title}
                touched={touched.title}
                handleChange={handleChange}
                value={values.title}
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
              <InputGroup
                label="Price"
                type="number"
                field="price"
                placeholder="Enter price"
                handleBlur={handleBlur}
                error={errors.price}
                touched={touched.price}
                handleChange={handleChange}
                value={values.price}
              ></InputGroup>
              <InputGroup
                label="Rating"
                type="number"
                field="rating"
                placeholder="Enter rating"
                handleBlur={handleBlur}
                error={errors.rating}
                touched={touched.rating}
                handleChange={handleChange}
                value={values.rating}
              ></InputGroup>
              <InputGroup
                label="Discount"
                type="number"
                field="discount"
                placeholder="Enter discount"
                handleBlur={handleBlur}
                error={errors.discount}
                touched={touched.discount}
                handleChange={handleChange}
                value={values.discount}
              ></InputGroup>
              <InputGroup
                label="DeliveryKit"
                type="text"
                field="deliveryKit"
                placeholder="Enter delivery kit"
                handleBlur={handleBlur}
                error={errors.deliveryKit}
                touched={touched.deliveryKit}
                handleChange={handleChange}
                value={values.deliveryKit}
              ></InputGroup>
              <CheckboxGroup
                label="Is Stock"
                type="checkbox"
                field="isStock"
                handleBlur={handleBlur}
                error={errors.isStock}
                touched={touched.isStock}
                handleChange={handleChange}
                checked={values.isStock}
              ></CheckboxGroup>
              <SelectGroup
                label="Category"
                field="categoryId"
                handleChange={handleChange}
                error={errors.categoryId}
                touched={touched.categoryId}
                handleBlur={handleBlur}
                options={categories}
                optionKey="id"
                optionLabel="name"
              ></SelectGroup>
              <div className="mt-5">
                <button
                  type="submit"
                  className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
                >
                  Create
                </button>
                <button
                  onClick={() => navigate(-1)}
                  data-modal-hide="default-modal"
                  type="button"
                  className="py-2.5 px-5 ms-3 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
                >
                  Cancel
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    </>
  );
}

export default ProductsCreatePage;
