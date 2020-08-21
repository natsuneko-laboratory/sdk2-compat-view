/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/
use std::ffi::{CStr, CString};

use yaml_rust::{Yaml, YamlLoader};

type CChar = libc::c_char;

struct FFIUtility {}

impl FFIUtility {
    pub fn to_r_string(raw: *const libc::c_char) -> Option<String> {
        unsafe {
            match CStr::from_ptr(raw).to_str() {
                Ok(s) => Some(s.to_owned()),
                _ => None,
            }
        }
    }

    pub fn to_c_string(string: String) -> *const libc::c_char {
        let string = CString::new(string).unwrap();
        string.into_raw()
    }

    pub fn free_c_string(ptr: *mut libc::c_char) {
        if ptr.is_null() {
            return;
        }

        unsafe {
            CString::from_raw(ptr);
        }
    }
}

pub struct YamlReader {
    inner: Vec<Yaml>,
}

impl YamlReader {
    fn new(source: String) -> Self {
        let loader = YamlLoader::load_from_str(&source).unwrap();

        YamlReader { inner: loader }
    }

    fn read_path(&self, path: String) -> Yaml {
        let docs = &self.inner[0];
        match path.parse::<usize>() {
            Ok(value) => docs[value].clone(), // access as string
            Err(_) => docs[path.as_str()].clone(),
        }
    }

    fn read_paths(&self, paths: Vec<String>) -> Yaml {
        let mut docs = &self.inner[0];

        for path in paths {
            docs = match path.parse::<usize>() {
                Ok(value) => &docs[value], // access as string
                Err(_) => &docs[path.as_str()],
            }
        }

        return docs.clone();
    }

    fn find_property(&self, path: String) -> YamlDocs {
        if path.contains(".") {
            return YamlDocs::new(self.read_paths(path.split(".").map(|w| w.to_owned()).collect()));
        }
        YamlDocs::new(self.read_path(path))
    }
}

pub struct YamlDocs {
    inner: Yaml,
}

impl YamlDocs {
    fn new(docs: Yaml) -> Self {
        YamlDocs { inner: docs }
    }

    fn read_path(&self, path: String) -> Yaml {
        match path.parse::<usize>() {
            Ok(value) => self.inner[value].clone(), // access as string
            Err(_) => self.inner[path.as_str()].clone(),
        }
    }

    fn read_paths(&self, paths: Vec<String>) -> Yaml {
        let mut docs = self.inner.clone();

        for path in paths {
            docs = match path.parse::<usize>() {
                Ok(value) => docs[value].clone(),
                Err(_) => docs[path.as_str()].clone(),
            };
        }

        docs
    }

    fn find_relative(&self, path: String) -> Self {
        if path.contains(".") {
            return YamlDocs::new(self.read_paths(path.split(".").map(|w| w.to_owned()).collect()));
        }

        YamlDocs::new(self.read_path(path))
    }

    // get value as ...

    fn as_bool(&self) -> bool {
        match self.inner.as_bool() {
            Some(boolean) => boolean,
            None => false,
        }
    }

    fn as_i64(&self) -> i64 {
        match self.inner.as_i64() {
            Some(ulong) => ulong,
            None => 0,
        }
    }

    fn as_f64(&self) -> f64 {
        match self.inner.as_f64() {
            Some(double) => double,
            None => 0 as f64,
        }
    }

    fn as_string(&self) -> String {
        match self.inner.as_str() {
            Some(string) => string.to_owned(),
            None => "".to_owned(),
        }
    }

    // matchers
    fn is_bad_value(&self) -> bool {
        self.inner.is_badvalue()
    }

    fn is_null(&self) -> bool {
        self.inner.is_null()
    }

    fn is_array(&self) -> bool {
        self.inner.is_array()
    }

    // others
    fn array_size(&self) -> u64 {
        let vec = match self.inner.as_vec() {
            Some(vec) => vec,
            None => return 0,
        };

        return vec.len() as u64;
    }
}

#[no_mangle]
pub unsafe extern "C" fn create_reader(raw: *const CChar) -> *mut YamlReader {
    let source = FFIUtility::to_r_string(raw).unwrap();
    Box::into_raw(Box::new(YamlReader::new(source)))
}

#[no_mangle]
pub unsafe extern "C" fn find_property(
    handle: *mut YamlReader,
    raw: *const CChar,
) -> *mut YamlDocs {
    let reader = &*handle;
    let path = FFIUtility::to_r_string(raw).unwrap();
    Box::into_raw(Box::new(reader.find_property(path)))
}

#[no_mangle]
pub unsafe extern "C" fn destroy_reader(handle: *mut YamlReader) {
    Box::from_raw(handle);
}

#[no_mangle]
pub unsafe extern "C" fn find_relative(handle: *mut YamlDocs, raw: *const CChar) -> *mut YamlDocs {
    let docs = &*handle;
    let path = FFIUtility::to_r_string(raw).unwrap();
    Box::into_raw(Box::new(docs.find_relative(path)))
}

#[no_mangle]
pub unsafe extern "C" fn as_bool(handle: *mut YamlDocs) -> bool {
    let docs = &*handle;
    docs.as_bool()
}

#[no_mangle]
pub unsafe extern "C" fn as_i64(handle: *mut YamlDocs) -> i64 {
    let docs = &*handle;
    docs.as_i64()
}

#[no_mangle]
pub unsafe extern "C" fn as_f64(handle: *mut YamlDocs) -> f64 {
    let docs = &*handle;
    docs.as_f64()
}

#[no_mangle]
pub unsafe extern "C" fn as_string(handle: *mut YamlDocs) -> *const CChar {
    let docs = &*handle;
    FFIUtility::to_c_string(docs.as_string())
}

#[no_mangle]
pub unsafe extern "C" fn is_bad_value(handle: *mut YamlDocs) -> bool {
    let docs = &*handle;
    docs.is_bad_value()
}

#[no_mangle]
pub unsafe extern "C" fn is_null(handle: *mut YamlDocs) -> bool {
    let docs = &*handle;
    docs.is_null()
}

#[no_mangle]
pub unsafe extern "C" fn is_array(handle: *mut YamlDocs) -> bool {
    let docs = &*handle;
    docs.is_array()
}

#[no_mangle]
pub unsafe extern "C" fn array_size(handle: *mut YamlDocs) -> u64 {
    let docs = &*handle;
    docs.array_size()
}

#[no_mangle]
pub unsafe extern "C" fn destroy_docs(handle: *mut YamlDocs) {
    Box::from_raw(handle);
}

#[no_mangle]
pub unsafe extern "C" fn free_string(handle: *mut CChar) {
    FFIUtility::free_c_string(handle);
}
