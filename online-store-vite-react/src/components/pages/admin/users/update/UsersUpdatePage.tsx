import { useNavigate, useParams } from "react-router-dom";
import * as Yup from "yup";
import { useEffect, useState } from "react";
import http_common from "../../../../../http_common.ts";
import { IUserUpdate } from "../../../../../entities/User.ts";
import { Form, Formik } from "formik";
import InputGroup from "../../../../../common/admin/InputGroup.tsx";
import ImageGroup from "../../../../../common/admin/ImageGroup.tsx";
import DropdownCheckboxGroup from "../../../../../common/admin/DropdownCheckboxGroup.tsx";
import { IRole } from "../../../../../entities/Role.ts";
import { ITablePermission } from "../../../../../entities/TablePermission.ts";
import { toast } from "react-toastify";
import { IModeratingTable } from "../../../../../entities/ModeratingTable.ts";
import { IoClose } from "react-icons/io5";

function UsersUpdatePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [selectedTablesIds, setSelectedTablesIds] = useState<number[]>([]);
  const [allRoles, setAllRoles] = useState<IRole[]>([]);
  const [allModeratingTables, setAllModeratingTables] = useState<
    IModeratingTable[]
  >([]);

  const [initialValues, setInitialValues] = useState<IUserUpdate>({
    position: "",
    email: "",
    userName: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    image: null,
    roles: [],
  });

  const userSchema = Yup.object().shape({
    email: Yup.string()
      .email("Invalid Email address")
      .required("Email is required")
      .max(255, "Email must be smaller"),
    userName: Yup.string()
      .required("User name is required")
      .min(3, "User name must be at least 3 characters")
      .max(255, "User name must be smaller"),
    firstName: Yup.string()
      .required("First name is required")
      .min(3, "First name must be at least 3 characters")
      .max(255, "First name must be smaller"),
    lastName: Yup.string()
      .required("Last name is required")
      .min(3, "Last name must be at least 3 characters")
      .max(255, "Last name must be smaller"),
    phoneNumber: Yup.string()
      .required("Phone number is required")
      .matches(/^[0-9]*$/, "Phone number must contain only digits")
      .max(255, "Phone number must be smaller"),
    image: Yup.mixed().required("Image is required"),
    roles: Yup.array()
      .required("At least one role must be selected")
      .min(1, "At least one role must be selected"),
  });

  const fetchModeratingTables = async () => {
    await http_common
      .get(`api/ModeratingTables`)
      .then((resp) => {
        setAllModeratingTables(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to moderating tables data");
        console.error("Error fetching moderating tables data:", error);
      });
  };

  const fetchRoles = async () => {
    await http_common
      .get(`api/Roles`)
      .then((resp) => {
        setAllRoles(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to fetch roles data");
        console.error("Error fetching roles data:", error);
      });
  };

  const fetchUser = async () => {
    await http_common
      .get(`api/Accounts/${id}`)
      .then(async (resp) => {
        const userData = resp.data;

        setSelectedTablesIds(
          userData.permissions.map(
            (p: ITablePermission) => p.moderatingTableId,
          ),
        );

        setInitialValues((prevValues) => ({
          ...prevValues,
          email: userData.email,
          firstName: userData.firstName,
          lastName: userData.lastName,
          userName: userData.userName,
          phoneNumber: userData.phoneNumber,
          image: null,
          roles: userData.roles,
        }));

        const imageResponse = await http_common.get(
          `/images/1200_${userData.image}`,
          {
            responseType: "arraybuffer",
          },
        );

        const imageBlob = new Blob([imageResponse.data], {
          type: "image/webp",
        });

        const imageFile = new File([imageBlob], userData.image, {
          type: "image/webp",
        });

        setInitialValues((prevValues) => ({
          ...prevValues,
          image: imageFile,
        }));
      })
      .catch((error) => {
        toast.error("Failed to fetch user data");
        console.error("Error fetching user data:", error);
      });
  };

  useEffect(() => {
    window.scrollTo({ top: 0 });
    fetchRoles();
    fetchUser();
    fetchModeratingTables();
  }, [id]);

  const handleSubmit = async (values: IUserUpdate) => {
    await userSchema.validate(values);

    await http_common
      .put(`api/Accounts/${id}`, values, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then(async () => {
        toast.success("User updated successfully");
        if (values.roles.includes("Moderator")) {
          await http_common
            .put(`api/TablePermissions/replace/${id}`, selectedTablesIds)
            .then(() => {
              toast.success("User table permissions updated successfully");
            })
            .catch((error) => {
              toast.error("Failed to update user table permissions");
              console.error("Error updating user table permissions:", error);
            });
        }
      })
      .catch((error) => {
        toast.error("Failed to update user data");
        console.error("Error updating user:", error);
      });

    navigate(-1);
  };

  return (
    <>
      <div className="p-5 sm:ml-64">
        <Formik
          initialValues={initialValues}
          onSubmit={handleSubmit}
          validationSchema={userSchema}
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
                  Update User
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
              <ImageGroup
                image={values.image}
                setFieldValue={setFieldValue}
                error={errors.image}
                touched={touched.image}
              ></ImageGroup>
              <DropdownCheckboxGroup
                label="Select roles"
                items={allRoles.map((role) => role.name)}
                selectedItems={values.roles}
                error={errors.roles}
                touched={touched.roles}
                handleBlur={handleBlur}
                onChange={(selectedRoles) =>
                  setFieldValue("roles", selectedRoles)
                }
              />
              {values.roles.includes("Moderator") && (
                <DropdownCheckboxGroup
                  label="Select tables for moderating"
                  items={allModeratingTables.map((table) => table.tableName)}
                  selectedItems={selectedTablesIds.map((id) => {
                    console.log(selectedTablesIds);
                    console.log(allModeratingTables);
                    const table = allModeratingTables.find((t) => t.id === id);
                    return table?.tableName || "";
                  })}
                  handleBlur={handleBlur}
                  onChange={(selectedTableNames) => {
                    const selectedIds = allModeratingTables
                      .filter((table) =>
                        selectedTableNames.includes(table.tableName),
                      )
                      .map((table) => table.id);
                    setSelectedTablesIds(selectedIds);
                  }}
                />
              )}
              <InputGroup
                label="Email"
                type="email"
                field="email"
                placeholder="Enter email"
                handleBlur={handleBlur}
                error={errors.email}
                touched={touched.email}
                handleChange={handleChange}
                value={values.email}
              ></InputGroup>
              <InputGroup
                label="UserName"
                type="text"
                field="userName"
                placeholder="Enter user name"
                handleBlur={handleBlur}
                error={errors.userName}
                touched={touched.userName}
                handleChange={handleChange}
                value={values.userName}
              ></InputGroup>
              <InputGroup
                label="First name"
                type="text"
                field="firstName"
                placeholder="Enter first name"
                handleBlur={handleBlur}
                error={errors.firstName}
                touched={touched.firstName}
                handleChange={handleChange}
                value={values.firstName}
              ></InputGroup>
              <InputGroup
                label="Last name"
                type="text"
                field="lastName"
                placeholder="Enter last name"
                handleBlur={handleBlur}
                error={errors.lastName}
                touched={touched.lastName}
                handleChange={handleChange}
                value={values.lastName}
              ></InputGroup>
              <InputGroup
                label="Phone number"
                type="tel"
                field="phoneNumber"
                placeholder="Enter phone number"
                handleBlur={handleBlur}
                error={errors.phoneNumber}
                touched={touched.phoneNumber}
                handleChange={handleChange}
                value={values.phoneNumber}
              ></InputGroup>
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

export default UsersUpdatePage;
